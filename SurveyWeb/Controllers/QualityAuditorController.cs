using SurveyWeb.Models;
using SurveyWeb.Models.QualityAuditor.SurveillancePerformance;
using SurveyWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class QualityAuditorController : Controller
    {
        // GET: QualityAuditor
        public ActionResult Dashboard()
        {
            //AuditorUserModel model = new AuditorUserModel();

            //return View(model);
            return View();
        }

        public ActionResult Surveillances()
        {
            int SurveyId = 0;
            var model = new SurveillancesViewModel();
            //Todo: Re-factor to proper use of Services Layer
            model.Surveillances = SurveillancePerformanceServices.GetSurveyorSurveillances(SurveyId);
            
            return View(model);
        }

        public ActionResult Observations()
        {
            int SurveyId = 0;
            var model = new ObservationsViewModel();
            //Todo: Re-factor to proper use of Services Layer
            model.Observations = SurveillancePerformanceServices.GetSurveyorObservations(SurveyId);

            return View(model);
        }
        public ActionResult FollowUps()
        {
            return View();
        }

    }
}