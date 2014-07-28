using SurveyWeb.Models;
using SurveyWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class LearningController : Controller
    {
        /// <summary>
        /// Review performance of Learning Module
        /// </summary>
        /// <param name="observation"></param>
        /// <returns></returns>
        public ActionResult ObservationClassifier(string observation)
        {
            //View will contain Classification and list of EP Choices
            StandardElementViewModel model = new StandardElementViewModel();
            if (observation != null && observation.Length > 0)
            {
                model.Observation = observation;
                model = ServiceManager.GetService<LearningServices>().Classify(observation);

            }

            return View(model);
        }


    }
}
