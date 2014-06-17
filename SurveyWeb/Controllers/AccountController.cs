using System.Collections;
using System.Configuration;
using System.Xml.Linq;
using Atum.Database.Surveillance.Models;
using Atum.Domain.Business;
using Atum.Domain.Common;
using Atum.Domain.Security.Domain;
using Atum.Utility;
using Atum.Utility.XML;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SurveyWeb.App_Start;
using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SurveyWeb.Services;

namespace SurveyWeb.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private AtumSurveillanceContext _dbContext = null;
        private ApplicationUserManager _userManager;
        private const int InviteeMaxCount = 6;

        public AccountController(ApplicationUserManager userManager) : this()
        {
            UserManager = userManager;
        }


        public AccountController()
        {
            _dbContext = new AtumSurveillanceContext();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName, };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);
                    //return RedirectToAction("Index", "Home");
                    
                    return RedirectToAction("ManageProfile", new {userName = model.UserName, email = model.Email});
                }
                else
                {
                    this.AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        //
        // GET: /Account/ManageProfile
        public ActionResult ManageProfile(string userName, string email)
        {
            var model = new PersonViewModel { UserId = userName };

            if (EmailHelper.IsValidEmail(email))
            {
                var domainName = EmailHelper.GetDomainName(email);

                var hospital = _dbContext.Hospitals.FirstOrDefault(m => m.DomainName == domainName);

                if (hospital != default(Hospital))
                {
                    model.Person.Hospital = hospital;
                }

                model.Email = email;
            }
            
            return View("Person", model);
        }

        //
        // POST: /Account/ManageProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageProfile(PersonViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Person Id does not exist, create a new one
                if (string.IsNullOrWhiteSpace(model.Person.Id))
                {
                    model.Person.Id = Guid.NewGuid().ToString("D");
                    _dbContext.Persons.Add(model.Person);
                }
                else
                {
                    _dbContext.Persons.Attach(model.Person);
                    _dbContext.Entry(model.Person).CurrentValues.SetValues(model.Person);
                    _dbContext.Entry(model.Person).State = EntityState.Modified;
                }

                // Hospital was not entered, create a new one
                if (string.IsNullOrWhiteSpace(model.Person.Hospital.Id))
                {
                    model.Person.Hospital.Id = Guid.NewGuid().ToString("D");
                    model.Person.Hospital.DomainName = EmailHelper.GetDomainName(model.Email);
                    _dbContext.Hospitals.Add(model.Person.Hospital);
                }
                else
                {
                    _dbContext.Hospitals.Attach(model.Person.Hospital);
                    _dbContext.Entry(model.Person.Hospital).CurrentValues.SetValues(model.Person.Hospital);
                    _dbContext.Entry(model.Person.Hospital).State = EntityState.Modified;
                }
                
                var user = _dbContext.AspNetUsers.FirstOrDefault(t => t.UserName == model.UserId);

                if (user != default(AspNetUser))
                {
                    user.Person = model.Person;
                }
                else
                {
                    this.AddErrors(new IdentityResult("User not found with id: " + model.UserId));
                }

                try
                {
                    _dbContext.SaveChanges();
                    
                    return RedirectToAction("Welcome", "Home");
                }
                catch (DbEntityValidationException e)
                {
                    this.AddErrors(e);
                }
            }

            // If we got this far, something failed, redisplay form
            return View("Person", model);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");

            var userName = User.Identity.GetUserName();

            var user = _dbContext.AspNetUsers.FirstOrDefault(m => m.UserName == userName);

            // _dbContext.Entry(user).Reference(m => m.Person).Load();
            
            var model = new PersonViewModel(user.Person) { UserId = userName};

            ViewBag.PersonViewModel = model;

            return View();
        }



        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        this.AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        this.AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
        public ActionResult DeleteUser(string userId)
        {
            var model = _dbContext.AspNetUsers.FirstOrDefault(user => user.Id == userId);

            return View("DeleteUser", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUser(AspNetUser model)
        {
            IList<UserLoginInfo> logins = await UserManager.GetLoginsAsync(model.Id);
            
            foreach (var login in logins)
            {
                IdentityResult result = await UserManager.RemoveLoginAsync(model.Id, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                if (!result.Succeeded)
                {
                    this.AddErrors(result);
                }
            }

            AspNetUser toDelete = _dbContext.AspNetUsers.FirstOrDefault(m => m.Id == model.Id);

            _dbContext.AspNetUsers.Remove(toDelete);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("UserHospitalIndex", "Hospital");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new AccountController.ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                this.AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[AccountController.XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        // GET: Account/LostPassword
        [AllowAnonymous]
        public ActionResult LostPassword()
        {
            return View();
        }

        // POST: Account/LostPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LostPassword(LostPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user;
                using (var context = _dbContext)
                {
                    var foundUserName = (from u in context.AspNetUsers
                              where u.Person.Email == model.Email
                              select u.UserName).FirstOrDefault();
                    if (foundUserName != null)
                    {
                        user = UserManager.FindByName(foundUserName.ToString());
                    }
                    else
                    {
                        user = null;
                    }
                }
                if (user != null)
                {
                    // Generate password token that will be used in the email link to authenticate user
                    // var token = WebSecurity.GeneratePasswordResetToken(user.UserName);
                    var token = EmailHelper.GenerateToken(user.UserName);
                    // Generate the html link sent via email
                    string resetLink = "<a href='"
                        + Url.Action("ResetPassword", "Account", new { rt = token }, "http") 
                        + "'>Reset Password Link</a>";
 
                    // Create an email with reset instructions
                    string baseUrl = Request.Url.Host == "localhost" ? "localhost.com" : Request.Url.Host;
                    string subject = "Reset your password for @" + baseUrl;
                    string body = "You link: " + resetLink;
                    string from = "donotreply@ " + baseUrl;

                    var mailService = ServiceManager.GetService<MailService>();
                    // Attempt to send the email
                    try
                    {
                        mailService.SendEmail(from, model.Email, subject, body, true, baseUrl);
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Issue sending email: " + e.Message);
                    }
                }         
                else // Email not found
                {
                    /* Note: You may not want to provide the following information
                    * since it gives an intruder information as to whether a
                    * certain email address is registered with this website or not.
                    * If you're really concerned about privacy, you may want to
                    * forward to the same "Success" page regardless whether an
                    * user was found or not. This is only for illustration purposes.
                    */
                    this.AddErrors(new IdentityResult("No user found by that email."));
                }
            }
         
            /* You may want to send the user to a "Success" page upon the successful
            * sending of the reset email link. Right now, if we are 100% successful
            * nothing happens on the page. :P
            */
            return View(model);
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string rt)
        {
            var model = new ResetPasswordModel();
            model.ReturnToken = rt;
            return View(model);
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                //bool resetResponse = WebSecurity.ResetPassword(model.ReturnToken, model.Password);

                var userName = EmailHelper.GetUsernameFromToken(model.ReturnToken);
                var user = UserManager.FindByName(userName);
                if (user != null)
                {
                    using (var store = new UserStore<ApplicationUser>())
                    {
                        store.SetPasswordHashAsync(user, UserManager.PasswordHasher.HashPassword(model.Password)).ContinueWith(t =>
                            UserManager.UpdateAsync(user)).Wait();
                        
                        _dbContext.SaveChanges();
                    }
                    ViewBag.Message = "Successfully Changed";
                }
                else
                {
                    ViewBag.Message = "Something went horribly wrong!";
                }
            }
            return View(model);
        }

        // GET: /Account/InvitePeople
        public ActionResult InvitePeople()
        {
            var invitees = new List<InvitePersonViewModel>();
            var userName = User.Identity.GetUserName();
            var person = _dbContext.AspNetUsers.FirstOrDefault(m => m.UserName == userName).Person;
            if (person != null)
            {
                for (int i = 0; i < InviteeMaxCount; i++)
                {
                    invitees.Add(new InvitePersonViewModel { Domain = EmailHelper.GetDomainName(person.Email) });
                }
            }
            
            return View(invitees);
        }
        
        // POST: /Account/InvitePeople
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InvitePeople(IEnumerable<InvitePersonViewModel> invitees)
        {
            // Create an email with reset instructions
            string baseUrl = Request.Url.Host == "localhost" ? "localhost.com" : Request.Url.Host;
            var subject = "Welcome to " + ConfigurationManager.AppSettings["WhiteLabel"];
            var from = "donotreply@" + baseUrl;

            var template = GetEmailTemplate(EmailTemplate.Invitation);

            try
            {
                var mailService = ServiceManager.GetService<MailService>();
                foreach (var item in invitees)
                {
                    // Verify that email was filled in, otherwise skip
                    if (string.IsNullOrWhiteSpace(item.Email))
                    {
                        continue;
                    }

                    // Send the email
                    string email = item.Email.Contains("@") ? item.Email : String.Join("@", item.Email, item.Domain);
                    mailService.SendEmail(from, email, subject, template.ToString(), true, baseUrl);
                }

                ViewBag.Message = "Invited People Successfully.";
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Issue sending email: " + e.Message);
            }

            return View(invitees);
        }

        public enum EmailTemplate
        {
            Invitation,
            ResetPassword,
            EventAssigned
        }


        public static XDocument GetEmailTemplate(EmailTemplate template)
        {
            string appPath = AppDomain.CurrentDomain.RelativeSearchPath;

            appPath = appPath + @"\..\RuleApp\";

            var emailFileName = string.Empty;
            if (template == EmailTemplate.Invitation)
            {
                emailFileName = "InvitationEmail.xml";
            }
            else if (template == EmailTemplate.ResetPassword)
            {
                emailFileName = "ResetPassword.xml";
            }
            else if (template == EmailTemplate.EventAssigned)
            {
                emailFileName = "EventAssigned.xml";
            }

            var result = XDocument.Load(appPath + @"Emails\" + emailFileName);

            return result;
        }
    }
}