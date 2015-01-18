using System.Configuration;
using System.Xml.Linq;
using Atum.Database.Surveillance.Models;
using Atum.Domain.Business;
using Atum.Domain.Security.Domain;
using Atum.Utility;
using Microsoft.AspNet.Identity;
using SurveyWeb.Models;
using SurveyWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    [Authorize]
    public class HospitalController : Controller
    {
        private const int InviteeMaxCount = 6;
        private static readonly List<Hospital> EmptyHospitalList = new List<Hospital>();
        private AtumSurveillanceContext _dbContext = null;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SurveillanceManagementServices _surveillanceService;
        private readonly AccountService _accountService;
        private readonly MailService _mailService;

        public HospitalController(UserManager<ApplicationUser> userManager, SurveillanceManagementServices surveillanceService, AccountService accountService)
        {
            _dbContext = new AtumSurveillanceContext();
            _userManager = userManager;
            _surveillanceService = surveillanceService;
            _accountService = accountService;
        }
        
        public ActionResult Index()
        {
            var model = _surveillanceService.GetHospitals();
            
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(Hospital model)
        {
            model.Id = Guid.NewGuid().ToString();

            _dbContext.Hospitals.Add(model);

            _dbContext.SaveChanges();
            
            return RedirectToAction("Index");
        }
        
        public ActionResult Edit(string id)
        {
            var model = _dbContext.Hospitals.First(item => item.Id == id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Hospital model)
        {
            var original = _dbContext.Hospitals.Find(model.Id);

            _dbContext.Entry(original).CurrentValues.SetValues(model);
            
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Details(string id)
        {
            var model = _surveillanceService.GetHospital(id);

            return View(model);
        }

        public ActionResult Delete(string id)
        {
            var model = _surveillanceService.GetHospital(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(Hospital model)
        {
            _surveillanceService.DeleteHospital(model.Id);            

            return RedirectToAction("Index");
        }

        public ActionResult UserHospitalIndex()
        {
            var listOnlyThisUserHospitals = ListOnlyThisUserHospitals();
            
            var model = GetUserHospitals(listOnlyThisUserHospitals);

            model = from a in _accountService.GetUsers().ToList()
                join b in model.ToList() on a.Id equals b.UserId
                let userName = a.UserName
                select new UserHospitalViewModel() {UserId = a.Id, UserName = userName};

            ViewBag.ShowAdminContent = _userManager.IsInRole(User.Identity.GetUserId(), "Administrator");
            ViewBag.ShowManagerContent = _userManager.IsInRole(User.Identity.GetUserId(), "Manager");
                        
            return View("UserHospitalIndex", model);
        }

        private bool ListOnlyThisUserHospitals()
        {
            var adminRole = _accountService.GetRole("Administrator");
            var userId = User.Identity.GetUserId();
            var aspNetUser = _accountService.GetUserWithRoles(userId);

            bool listOnlyThisUserHospitals = !aspNetUser.AspNetRoles.Contains(adminRole);
            return listOnlyThisUserHospitals;
        }

        private IEnumerable<UserHospitalViewModel> GetUserHospitals(bool listOnlyThisUserHospitals)
        {
            var availableHospitals = _surveillanceService.GetHospitals().ToList();
            var availableRoles = _accountService.GetRoles().ToList();

            // If user is not system admin, list only users from the user's hospitals list
            
            if (listOnlyThisUserHospitals)
            {
                var userId = User.Identity.GetUserId();
                var userHospitalIds = _accountService.GetUserHospitals(userId).Select(m => m.HospitalId).ToList();
                availableHospitals = availableHospitals.Where(m => userHospitalIds.Contains(m.Id)).ToList();
            }

            var currentUserId = User.Identity.GetUserId();
            var aspNetUser = _accountService.GetUser(currentUserId);
            var adminRole = _accountService.GetRole("Administrator");
            if (!aspNetUser.AspNetRoles.Contains(adminRole))
            {
                availableRoles.Remove(adminRole);
            }

            var userHospitalMap = new Dictionary<string, List<Hospital>>();
            var hospitals = from c in _accountService.GetUsers()
                join a in _accountService.GetUserHospitals() on c.Id equals a.UserId into uh
                from userHospital in uh.DefaultIfEmpty()
                join b in availableHospitals on (userHospital != null ? userHospital.HospitalId : string.Empty) equals b.Id into hospital
                select new {UserId = c.Id, Hospital = hospital};
            
            foreach (var userHospital in hospitals.ToList())
            {
                if (!userHospitalMap.ContainsKey(userHospital.UserId))
                {
                    userHospitalMap.Add(userHospital.UserId, new List<Hospital>());
                }

                userHospitalMap[userHospital.UserId].AddRange(userHospital.Hospital);
            }
            
            foreach (var userId in userHospitalMap.Keys)
            {
                yield return new UserHospitalViewModel
                    {
                        AvailableHospitals = availableHospitals,
                        Hospitals = userHospitalMap[userId],
                        UserId = userId,

                        AvailableRoles = availableRoles,
                        Roles = _accountService.GetUserWithRoles(userId).AspNetRoles
                    };
            }
        }
        
        public ActionResult CreateUserHospital(string userId, string hospitalId)
        {
            _accountService.AddUserHospital(userId, hospitalId);            

            return RedirectToAction("UserHospitalIndex", "Hospital");
        }

        public ActionResult EditUserHospital(string userId)
        {
            var availableHospitals = _dbContext.Hospitals.ToList();
            var availableRoles = _dbContext.AspNetRoles.ToList();

            var listOnlyThisUserHospitals = ListOnlyThisUserHospitals();

            var model = GetUserHospitals(listOnlyThisUserHospitals).FirstOrDefault(item => item.UserId == userId);

            var aspNetUser = _dbContext.AspNetUsers.First(user => user.Id == userId);
            ViewBag.UserName = aspNetUser.UserName;

            if (model != default(UserHospitalViewModel))
                model.LockoutEnabled = aspNetUser.LockoutEnabled;

            // If not system admin, remove admin role from available roles
            var adminRole = availableRoles.FirstOrDefault(m => m.Name == "Administrator");
            if (!aspNetUser.AspNetRoles.Contains(adminRole))
            {
                availableRoles.Remove(adminRole);
            }

            // If user has no user to hospital association records, create a viewmodel with empty lists
            if (model != default(UserHospitalViewModel))
            {
                return View("UserHospitalEdit",
                                   new UserHospitalViewModel
                                       {
                                           UserId = userId,
                                           LockoutEnabled = aspNetUser.LockoutEnabled,
                                           AvailableHospitals = availableHospitals,
                                           Hospitals = EmptyHospitalList,
                                           AvailableRoles = availableRoles,
                                           Roles = aspNetUser.AspNetRoles
                                       });
            }
            
            // Otherwise return the viewmodel
            return View("UserHospitalEdit", model);
        }

        [HttpPost]
        public ActionResult EditUserHospital(UserHospitalViewModel viewModel)
        {
            var listOnlyUserHospitals = ListOnlyThisUserHospitals();

            var model = GetUserHospitals(listOnlyUserHospitals).Where(item => item.UserId == viewModel.UserId);

            var userHospitalViewModel = model.FirstOrDefault();

            var aspNetUser =_accountService.GetUser(viewModel.UserId);

            if (aspNetUser.LockoutEnabled != viewModel.LockoutEnabled)
            {
                aspNetUser.LockoutEnabled = viewModel.LockoutEnabled;
                _accountService.UpdateUser(aspNetUser);
            }

            if (viewModel.SelectedHospitals != null)
            {
                // Remove unselected items
                if (userHospitalViewModel != null)
                {
                    foreach (var item in userHospitalViewModel.Hospitals)
                    {
                        if (viewModel.SelectedHospitals.Count(hospitalId => hospitalId == item.Id) == 0)
                        {
                            var toDelete = _dbContext.UserHospitals.First(userHospital => userHospital.HospitalId == item.Id && userHospital.UserId == viewModel.UserId);

                            _dbContext.UserHospitals.Remove(toDelete);
                        }
                    }
                }

                // Add newly selected items
                foreach (var hospitalId in viewModel.SelectedHospitals)
                {
                    if (userHospitalViewModel == null || userHospitalViewModel.Hospitals.Count(hospital => hospital.Id == hospitalId) == 0)
                    {
                        _dbContext.UserHospitals.Add(new UserHospital { HospitalId = hospitalId, UserId = viewModel.UserId });
                    }
                }

                _dbContext.SaveChanges();
            }

            if (viewModel.SelectedRoles != null)
            {
                // Remove unselected items
                if (userHospitalViewModel != null)
                {
                    foreach (var item in userHospitalViewModel.Roles.ToList())
                    {
                        if (viewModel.SelectedRoles.Count(roleId => roleId == item.Id) == 0)
                        {
                            var toDelete = _dbContext.AspNetUsers.First(user => user.Id == viewModel.UserId).AspNetRoles.First(userRole => userRole.Id == item.Id);

                            _dbContext.AspNetUsers.First(user => user.Id == viewModel.UserId).AspNetRoles.Remove(toDelete);
                        }
                    }
                }

                // Add newly selected items
                foreach (var roleId in viewModel.SelectedRoles)
                {
                    if (userHospitalViewModel == null || userHospitalViewModel.Roles.Count(role => role.Id == roleId) == 0)
                    {
                        aspNetUser.AspNetRoles.Add(_dbContext.AspNetRoles.First(aspNetRole => aspNetRole.Id == roleId));
                    }
                }

                _dbContext.SaveChanges();
            }

            return EditUserHospital(viewModel.UserId);
        }
        
        public ActionResult DeleteUserHospital(string userId, string hospitalId)
        {
            var model = _dbContext.UserHospitals.Find(new[] {userId, hospitalId});

            _dbContext.UserHospitals.Remove(model);

            _dbContext.SaveChanges();

            return RedirectToAction("UserHospitalIndex", "Hospital");
        }

        // GET: /Hospital/CreateUsers
        public ActionResult CreateUsers()
        {
            var invitees = new List<InvitePersonViewModel>();
            var userName = User.Identity.GetUserName();
            var person = _accountService.GetUsers().FirstOrDefault(m => m.UserName == userName).Person;
            if (person != null)
            {
                for (int i = 0; i < InviteeMaxCount; i++)
                {
                    invitees.Add(new InvitePersonViewModel { Domain = EmailHelper.GetDomainNameFromEmail(person.Email) });
                }
            }

            return View(invitees);
        }

        // POST: /Hospital/CreateUsers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUsers(IEnumerable<InvitePersonViewModel> invitees)
        {
            // Create an email with reset instructions
            string domainName = EmailHelper.GetDomainNameFromHost(Request.Url.Host);
            string baseUrl = String.Concat(domainName, ":", Request.Url.Port);
            var subject = "Welcome to " + ConfigurationManager.AppSettings["WhiteLabel"];
            var from = "donotreply@" + domainName;

            var template = GetEmailTemplate(EmailTemplate.Invitation);
            string body = template.ToString();

            string userName = User.Identity.GetUserId();
            var person = _accountService.GetUsers().FirstOrDefault(m => m.Id == userName).Person;

            var appPath = Request.ApplicationPath;

            var token = EmailHelper.GenerateToken(userName, 14 * 24, new string[] { "HospitalId=" + person.HospitalId });
            body = body.Replace("{{REGISTRATION_TOKEN}}", token);
            body = body.Replace("{{APP_PATH}}", appPath);

            try
            {
                foreach (var item in invitees)
                {
                    // Verify that email was filled in, otherwise skip
                    if (string.IsNullOrWhiteSpace(item.Email))
                    {
                        continue;
                    }

                    // Send the email
                    string to = item.Email.Contains("@") ? item.Email : String.Join("@", item.Email, item.Domain);
                    _mailService.SendEmail(from, to, subject, body, true, baseUrl);
                }

                ViewBag.Message = "Invited People Successfully.";
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Issue sending email: " + e.Message);
            }

            return View(invitees);
        }

        public XDocument GetEmailTemplate(EmailTemplate template)
        {
            return _mailService.GetEmailTemplate(template);
        }
    }
}
