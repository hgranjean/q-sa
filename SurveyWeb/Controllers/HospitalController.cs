using Atum.Database.Surveillance.Models;
using Atum.Domain.Business;
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
        private AtumSurveillanceContext _dbContext = null;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SurveillanceService _surveillanceService;
        private readonly AccountService _accountService;

        public HospitalController(UserManager<ApplicationUser> userManager, SurveillanceService surveillanceService, AccountService accountService)
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
                        
            ViewBag.Users = from a in _accountService.GetUsers().ToList()
                            join b in model.ToList() on a.Id equals b.UserId
                            select a;

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
            var hospitals = from a in _accountService.GetUserHospitals().ToList()
                            join b in availableHospitals on a.HospitalId equals b.Id
                            select a;            

            foreach (var userHospital in hospitals.ToList())
            {
                if (!userHospitalMap.ContainsKey(userHospital.UserId))
                {
                    userHospitalMap.Add(userHospital.UserId, new List<Hospital>());
                }

                userHospitalMap[userHospital.UserId].Add(userHospital.Hospital);
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

            var model = GetUserHospitals(listOnlyThisUserHospitals).Where(item => item.UserId == userId);

            var aspNetUser = _dbContext.AspNetUsers.First(user => user.Id == userId);
            ViewBag.UserName = aspNetUser.UserName;

            // If not system admin, remove admin role from available roles
            var adminRole = availableRoles.FirstOrDefault(m => m.Name == "Administrator");
            if (!aspNetUser.AspNetRoles.Contains(adminRole))
            {
                availableRoles.Remove(adminRole);
            }

            // If user has no user to hospital association records, create a viewmodel with empty lists
            if (!model.Any())
            {
                return View("UserHospitalEdit",
                                   new UserHospitalViewModel
                                       {
                                           UserId = userId,

                                           AvailableHospitals = availableHospitals,
                                           Hospitals = new List<Hospital>(),
                                           AvailableRoles = availableRoles,
                                           Roles = aspNetUser.AspNetRoles
                                       });
            }
            
            // Otherwise return the viewmodel
            return View("UserHospitalEdit", model.First());
        }

        [HttpPost]
        public ActionResult EditUserHospital(UserHospitalViewModel viewModel)
        {
            var listOnlyUserHospitals = ListOnlyThisUserHospitals();

            var model = GetUserHospitals(listOnlyUserHospitals).Where(item => item.UserId == viewModel.UserId);

            var userHospitalViewModel = model.FirstOrDefault();

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
                        var aspNetUser = _dbContext.AspNetUsers.First(user => user.Id == viewModel.UserId);

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
    }
}
