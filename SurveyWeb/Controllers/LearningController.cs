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
        private const string DefaultTrainingDocumentIdentifier = "EC.01.01.01";
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
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ObservationClass", model);
            }
            return View(model);
        }

        public ActionResult NBTrainingDocument(string trainingDocumetId) 
        {   
            trainingDocumetId = string.IsNullOrEmpty(trainingDocumetId) ? DefaultTrainingDocumentIdentifier: trainingDocumetId;

            TrainingDocumentViewModel model = _learningService.GetTrainingDocument(trainingDocumetId);
            
            if (Request.IsAjaxRequest())
            {
                return PartialView("_NBTrainingDocumentText", model);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult NBTrainingDocument(TrainingDocumentViewModel model)
        {
            model = _learningService.SaveTrainingDocument(model);

            return View(model);
        }
    }
}
