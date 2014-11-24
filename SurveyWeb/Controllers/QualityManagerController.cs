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
            return View();
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

        public ActionResult SurveillanceTemplates()
        {
            return View();
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