using Microsoft.AspNet.Identity;
using SurveyWeb.Models;
using SurveyWeb.Repository;
using System.Configuration;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ITaskRepository taskRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(ITaskRepository taskRepository, UserManager<ApplicationUser> userManager)
        {
            this.taskRepository = taskRepository;
            this.userManager = userManager;
        }                    
        
        protected override void Dispose(bool disposing)
        {
            // if (disposing && userManager != null)
            //{
                // userManager.Dispose();
                //userManager = null;
            //}
            base.Dispose(disposing);
        }
        
        public ActionResult Index()
        {
            if (userManager.IsInRole(User.Identity.GetUserId(), "Administrator"))
            {
                return RedirectToAction("Dashboard", "QTrackerAdmin");

            }
            else if (userManager.IsInRole(User.Identity.GetUserId(), "Manager"))
            {
                return RedirectToAction("Dashboard", "QualityManager");
                
            }
            else if (userManager.IsInRole(User.Identity.GetUserId(),"Team Member"))
            {
                return RedirectToAction("Dashboard", "QualityAuditor");
                
            }
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

            ViewBag.ShowAdminContent = userManager.IsInRole(User.Identity.GetUserId(), "Administrator");
            ViewBag.ShowManagerContent = userManager.IsInRole(User.Identity.GetUserId(), "Manager");

            return View();
        }

        public ActionResult Search(string query)
        {
            return View();
        }

        

        
    }
}