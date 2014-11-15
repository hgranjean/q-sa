using Atum.Domain.QualityManagement.Healthcare.JointCommission;
using SurveyWeb.Models;
using SurveyWeb.Services;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class LearningController : Controller
    {
        private const string DefaultTrainingDocumentIdentifier = "EC.01.01.01";
        private readonly LearningServices _learningService;
        private readonly StandardsManagementServices _standardManagementService;

        public LearningController(LearningServices learningService, StandardsManagementServices standardsManagementService)
        {
            _learningService = learningService;
            _standardManagementService = standardsManagementService;
        }


        public ActionResult StandardTraining(int? id)
        {
            var model = new StandardDocumentViewModel();
            if (id.HasValue)
            {
                //Load Document
                model = _standardManagementService.LoadDocument(id);
            }
            return View(model);
        }


        /// <summary>
        /// Review performance of Learning Module
        /// </summary>
        /// <param name="observation"></param>
        /// <returns></returns>
        public ActionResult ObservationClassifier(string observationText)
        {
            // Contract.Assert(!string.IsNullOrWhiteSpace(observation));

            //View will contain Classification and list of EP Choices
            StandardElement model = null;
            
            if (!string.IsNullOrWhiteSpace(observationText))
            {
                model = _learningService.Classify(observationText);
            }
            else
            {
                model = new StandardElement { Observation = observationText };
            }

            var viewModel = new StandardElementViewModel(model);

            // viewModel.Observation += Request.IsAjaxRequest().ToString();

            // if (Request.IsAjaxRequest())
            // {
                return PartialView("_ObservationClass", viewModel);
            // }
            // return View(viewModel);
        }

        //public ActionResult NBTrainingDocument(string chapterId, object dummy)
        //{
        //    string trainingDocumetId = null;
        //    trainingDocumetId = string.IsNullOrEmpty(trainingDocumetId) ? DefaultTrainingDocumentIdentifier : trainingDocumetId;
        //    string classifierId = trainingDocumetId.Split('.')[0];
        //    TrainingDocumentViewModel model = _learningService.GetTrainingDocument(classifierId, trainingDocumetId);

        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("_NBTrainingDocumentText", model);
        //    }

        //    return View(model);

        //}

        public ActionResult NBTrainingDocument(string standardId, string trainingDocumetId) 
        {   
//            string classifierId = trainingDocumetId.Split('.')[0];
            string defaultTrainingDocumetId = _learningService.GetDefaultTrainingDocumentId(standardId);
            trainingDocumetId = string.IsNullOrEmpty(trainingDocumetId) ? defaultTrainingDocumetId : trainingDocumetId;



            TrainingDocumentViewModel model = _learningService.GetTrainingDocument(standardId, trainingDocumetId);
            
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
