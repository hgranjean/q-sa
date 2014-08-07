using SurveyWeb.Models;
using SurveyWeb.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class LearningController : Controller
    {
        private readonly LearningServices _learningService;
        
        public LearningController(LearningServices learningService)
        {
            _learningService = learningService;
        }

        /// <summary>
        /// Review performance of Learning Module
        /// </summary>
        /// <param name="observation"></param>
        /// <returns></returns>
        public ActionResult ObservationClassifier(string observation)
        {
            Contract.Assert(!string.IsNullOrWhiteSpace(observation));

            //View will contain Classification and list of EP Choices
            StandardElementViewModel model = null;
            
            if (!string.IsNullOrWhiteSpace(observation))
            {             
                model = _learningService.Classify(observation);
            }
            else
            {
                model = new StandardElementViewModel { Observation = observation };
            }

            return View(model);
        }

    }
}
