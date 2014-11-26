using Atum.Domain.SurveyManagement;
using SurveyWeb.Models.QualityManager;
using SurveyWeb.Models.QualityManager.TemplatesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class QualityManagerController : Controller
    {
        // GET: QualityManager
        public ActionResult Dashboard()
        {
            ManagerUserModel model = new ManagerUserModel();
            
            return View(model);
        }

        //Manage Surveillance
        public ActionResult ManageSurveillances()
        {
            return View();
        }
        
        /*Manage Surveillances Subtasks*/
        public ActionResult TrackFollowUps()
        {
            return View();
        }


        /// <summary>
        /// View List of Surveys
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveillanceTemplates()
        {
            var model = new SurveysViewModel { Surveys = new Surveys() };

            //model.Surveys.AddRange(surveys);

            //ViewBag.ShowAdminContent = _userManager.IsInRole(User.Identity.GetUserId(), "Administrator");
            //ViewBag.ShowSurveyorContent = _userManager.IsInRole(User.Identity.GetUserId(), "Team Member");

            return View(model);
        }

        /*Surveillance Templates Subtasks*/
        public ActionResult ManageStandards()
        {
            return View();
        }

        public ActionResult SurveillanceSchedule()
        {
            return View();
        }
        /*Surveillance Schedule Subtasks*/
        public ActionResult ManageResources()
        {
            return View();
        }
        
        public ActionResult SurveillanceCompletion()
        {
            return View();
        }
        public ActionResult ManageAlerts()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IdentifyTrends()
        {
            return View();
        }


        public ActionResult ManagePIPrograms()
        {
            return View();
        }

    }
}