using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SurveyWeb.Models;

namespace SurveyWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController()
           : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }
        
        public ActionResult Index()
        {
            return RedirectToAction("Dashboard", "Surveillance");
        }

        public ActionResult Welcome()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "The application description page.";
            ViewBag.UserSupportLink = ConfigurationManager.AppSettings["UserSupportUrl"];

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "The contact page.";

            return View();
        }
        
        public ActionResult Settings()
        {
            ViewBag.Message = "The settings page.";

            ViewBag.ShowAdminContent = UserManager.IsInRole(User.Identity.GetUserId(), "Administrator");
            ViewBag.ShowManagerContent = UserManager.IsInRole(User.Identity.GetUserId(), "Manager");

            return View();
        }
    }
}