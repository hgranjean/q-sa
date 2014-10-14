using System.Data.Entity;
using System.Data.Entity.Validation;
using Atum.Database.Surveillance.Models;
using Atum.Domain;
using Atum.Domain.Common;
using Atum.Domain.Healthcare;
using Atum.Domain.Security.Domain;
using Atum.Domain.SurveyManagement;
using Atum.Utility.XML;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SurveyWeb.Models;
using SurveyWeb.RuleApp;
using SurveyWeb.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Atum.Utility;
using System.Diagnostics.Contracts;
using SurveyWeb.Repository;

namespace SurveyWeb.Controllers
{
    /// <summary>
    /// Survey Management
    /// Survey Template Definitions etc...
    /// </summary>
    public class SurveyController : Controller
    {        
        private readonly SurveyService _surveyService;        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPersistenceServices _persistenceService;
        private readonly StandardsManagementServices _standardManagementService;

        public SurveyController(            
            SurveyService surveyService,
            IPersistenceServices persistenceService,
            UserManager<ApplicationUser> userManager,
            StandardsManagementServices standardManagementService)
        {            
            _surveyService = surveyService;        
            _userManager = userManager;
            _persistenceService = persistenceService;
            _standardManagementService = standardManagementService;
        }

        //
        // GET: /Survey/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// View List of Surveys
        /// </summary>
        /// <returns></returns>
        public ActionResult Surveys()
        {            
            var surveys = _persistenceService.GetSurveys();            

            var model = new SurveysViewModel { Surveys = new Surveys() };
            
            model.Surveys.AddRange(surveys);

            ViewBag.ShowAdminContent = _userManager.IsInRole(User.Identity.GetUserId(), "Administrator");
            ViewBag.ShowSurveyorContent = _userManager.IsInRole(User.Identity.GetUserId(), "Team Member");

            return View(model);
        }

        /// <summary>
        /// Show New Survey Screen
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public ActionResult Create()
        {
            var survey = new Survey();

            var viewModel = new SurveyViewModel(survey);

            _persistenceService.SaveSurvey(survey);

            return View("SurveyDesign", viewModel);
        }
        
        /// <summary>
        /// Show New Survey Screen
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Survey survey)
        {
            var viewModel = new SurveyViewModel(survey);

            _persistenceService.SaveSurvey(survey);

            return View("SurveyDesign", viewModel);
        }
                
        public ActionResult CreateQuestionGroup(int surveyId)
        {
            var survey = _persistenceService.GetSurvey(surveyId);

            var surveyViewModel = new SurveyViewModel(survey);

            survey.EnsureQuestionGroups();
            
            int newGroupIndex = survey.QuestionGroups.Count() + 1;

            survey.AddQuestionGroup("New Group " + newGroupIndex);

            surveyViewModel = new SurveyViewModel(survey);

            _persistenceService.SaveSurvey(surveyViewModel.GetUpdatedSurvey());
            
            return View("SurveyDesign", surveyViewModel);
        }
               
        /// <summary>
        /// View Edit Question Group Screen/Page
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public ActionResult EditQuestionGroup(int surveyId, int groupId)
        {            
            var survey = _persistenceService.GetSurvey(surveyId);

            ViewBag.QuestionGroupId = groupId;

            var viewModel = new QuestionGroupViewModel(survey.QuestionGroups[groupId])
            {
                SurveyId = surveyId,
                Number = groupId,
                AvailableTOCs = _standardManagementService.GetTOCs()
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditQuestionGroupText(int surveyId, int questionGroupId, string value)
        {
            var survey = _persistenceService.GetSurvey(surveyId);

            survey.QuestionGroups[questionGroupId].Title = value;

            _persistenceService.SaveSurvey(survey);

            return new JsonResult{ Data = "success"};
        }



        /// <summary>
        /// Update an Edited Question Group
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditQuestionGroup(QuestionGroupViewModel viewModel)
        {            
            var survey = _persistenceService.GetSurvey(viewModel.SurveyId);

            if (viewModel.QuestionGroup != null)
            {
                survey.QuestionGroups[viewModel.Number] = viewModel.QuestionGroup;
            }
            
            _persistenceService.SaveSurvey(survey);

            var surveyViewModel = new SurveyViewModel(survey);

            return View("SurveyDesign", surveyViewModel);
        }

        /// <summary>
        /// Show Delete Question Group View.
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public ActionResult DeleteQuestionGroup(int surveyId, int groupId)
        {            
            var survey = _persistenceService.GetSurvey(surveyId);

            ViewBag.QuestionGroupId = groupId;

            var viewModel = new QuestionGroupViewModel(survey.QuestionGroups[groupId])
            {
                SurveyId = surveyId,
                Number = groupId
            };

            return View(viewModel);
        }
        /// <summary>
        /// Delete a Question Group from a Survey 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteQuestionGroup(QuestionGroupViewModel viewModel)
        {            
            var survey = _persistenceService.GetSurvey(viewModel.SurveyId);

            if (viewModel.QuestionGroup != null)
            {
                var questionGroupToDelete = survey.QuestionGroups[viewModel.Number];
                survey.QuestionGroups.Remove(questionGroupToDelete.Number);
            }

            _persistenceService.SaveSurvey(survey);

            var surveyViewModel = new SurveyViewModel(survey);

            return View("SurveyDesign", surveyViewModel);
        }

        /// <summary>
        /// Returns existing survey template or a new one
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveyDesign(int id)
        {
            //Using default SurveyType of Surveillance vs Evaluation, Assessment, Audit
            
            var survey = _persistenceService.GetSurveys().FirstOrDefault(m => m.ID == id);

            var model = new SurveyViewModel(survey);

            return View(model);
        }

        [HttpPost]
        public ActionResult Save(SurveyViewModel viewModel)
        {
            if (viewModel.Survey.Guid == Guid.Empty)
            {
                viewModel.Survey.Guid = Guid.NewGuid();

                var surveyEntry = new SurveyEntry
                {
                    Id = viewModel.Survey.Guid.ToString("d"),
                    Title = viewModel.Survey.Title
                };
                
                surveyEntry = _surveyService.AddSurvey(surveyEntry);
            }
            else
            {
                var id = viewModel.Survey.Guid.ToString("d");
                
                var surveyEntry = _surveyService.GetSurvey(id);

                surveyEntry.Title = viewModel.Survey.Title;
            }
            var questionGroups = viewModel.QuestionGroupsViewModel.ConvertAll(m => m.QuestionGroup);
            viewModel.Survey.QuestionGroups = new QuestionGroups();
            questionGroups.ForEach(m => viewModel.Survey.QuestionGroups.AddOrUpdate(m.Number, m));

            _persistenceService.SaveSurvey(viewModel.Survey);            

            return View(viewModel);
        }


        public ActionResult DeleteSurvey(int id)
        {            
            var model = _persistenceService.GetSurvey(id);
                        
            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteSurvey(SurveyViewModel model)
        {            
            _persistenceService.DeleteSurvey(model.Survey.ID.ToString());

            return RedirectToAction("Surveys");
        }

        [HttpPost]
        /// <summary>
        /// Add a Question to a Survey
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="questionGroupId"></param>
        /// <returns></returns>
        public ActionResult AddQuestion(int surveyId, int questionGroupId)
        {            
            var survey = _persistenceService.GetSurvey(surveyId);

            var questionGroup = survey.QuestionGroups[questionGroupId];

            var question = questionGroup.AddQuestion(string.Empty, QuestionType.SelectOne);
            
            _persistenceService.SaveSurvey(survey);
            
            var viewModel = new QuestionViewModel(question);

            viewModel.AvailableTOCs = _standardManagementService.GetTOCs();

            return PartialView("EditorTemplates/QuestionViewModel", viewModel);
        }
                
        public ActionResult EditQuestion(int surveyId, int questionId)
        {
            var ajax = Request.IsAjaxRequest();

            var survey = _persistenceService.GetSurvey(surveyId);
            
            var questionGroup = from a in survey.QuestionGroups
                           where a.Value.Questions.Exists(m=>m.Number == questionId)
                           select a;
            var question = questionGroup.First().Value.Questions.First(m => m.Number == questionId);
                        
            var viewModel = new QuestionViewModel(question);
            viewModel.SurveyId = surveyId;
            viewModel.AvailableTOCs = _standardManagementService.GetTOCs();
            viewModel.QuestionGroupNumber = questionGroup.First().Value.Number;

            if (ajax)
            {
                return PartialView(viewModel);
            }

            return View(viewModel);
        }

        public PartialViewResult ShowQuestion(int surveyId, int questionGroupId, int questionId)
        {
            var survey = _persistenceService.GetSurvey(surveyId);

            var questionGroup = survey.QuestionGroups[questionGroupId];

            var question = questionGroup.Questions.First(m => m.Number == questionId);

            var viewModel = new QuestionViewModel(question);
            viewModel.SurveyId = surveyId;
            viewModel.AvailableTOCs = _standardManagementService.GetTOCs();
            viewModel.QuestionGroupNumber = questionGroup.Number;
            
            return PartialView("EditorTemplates/QuestionViewModel", viewModel);
        }

        [HttpPost]
        public PartialViewResult EditQuestion(QuestionViewModel viewModel)
        {
            var survey = _persistenceService.GetSurvey(viewModel.SurveyId);

            survey.QuestionGroups[viewModel.QuestionGroupNumber].Questions.First(m => m.Number == viewModel.Number).Text = viewModel.Text;
            survey.QuestionGroups[viewModel.QuestionGroupNumber].Questions.First(m => m.Number == viewModel.Number).TOCReference = viewModel.TOCReference;
            survey.QuestionGroups[viewModel.QuestionGroupNumber].Questions.First(m => m.Number == viewModel.Number).QuestionType = viewModel.QuestionType;

            _persistenceService.SaveSurvey(survey);

            // TOCs are too complex to carry as a payload between the posts
            viewModel.AvailableTOCs = _standardManagementService.GetTOCs();

            return PartialView("EditorTemplates/QuestionViewModel", viewModel);
        }

        [HttpPost]
        /// <summary>
        /// Add a Question to a Survey
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="questionGroupId"></param>
        /// <returns></returns>
        public ActionResult DeleteQuestion(int surveyId, int questionGroupId, int questionId)
        {
            var survey = _persistenceService.GetSurvey(surveyId);

            var questionGroup = survey.QuestionGroups[questionGroupId];

            var question = questionGroup.Questions.First(m => m.Number == questionId);

            questionGroup.Questions.Remove(question);

            _persistenceService.SaveSurvey(survey);
            
            return new JsonResult { Data = "success" };
        }
	}
}