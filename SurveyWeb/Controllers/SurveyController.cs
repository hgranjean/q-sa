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
        public ActionResult Create(Survey survey)
        {
            var viewModel = new SurveyViewModel(survey);

            viewModel.Save(_persistenceService);

            return View(viewModel);
        }

        public ActionResult CreateQuestionGroup(int surveyId)
        {            
            var survey = _persistenceService.GetSurvey(surveyId);

            return View(new SurveyViewModel(survey));
        }

        [HttpPost]
        public ActionResult CreateQuestionGroup(SurveyViewModel surveyViewModel)
        {            
            var survey = _persistenceService.GetSurvey(Convert.ToInt32(surveyViewModel.Survey.ID));

            if (survey.QuestionGroups == null)
            {
                survey.QuestionGroups = new QuestionGroups();
            }
            int newGroupIndex = survey.QuestionGroups.Count() + 1;

            survey.AddQuestionGroup("New Group " + newGroupIndex);

            surveyViewModel = new SurveyViewModel(survey);

            surveyViewModel.Save(_persistenceService);

            return View("SurveyDesign", surveyViewModel);
        }
               
        /// <summary>
        /// View Edit Question Group Screen/Page
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public ActionResult EditQuestionGroup(string surveyId, string groupId)
        {            
            var survey = _persistenceService.GetSurveys().First(item => item.ID.ToString() == surveyId);

            ViewBag.QuestionGroupId = groupId;

            var viewModel = new QuestionGroupViewModel(survey.QuestionGroups[int.Parse(groupId)])
            {
                SurveyId = surveyId,
                Number = int.Parse(groupId),
                AvailableTOCs = _standardManagementService.GetTOCs()
            };
            return View(viewModel);
        }


        /// <summary>
        /// Update an Edited Question Group
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditQuestionGroup(QuestionGroupViewModel viewModel)
        {            
            var survey = _persistenceService.GetSurveys().First(item => item.ID.ToString() == viewModel.SurveyId);

            if (viewModel.QuestionGroup != null && viewModel.QuestionGroup.Questions != null)
            {
                survey.QuestionGroups[viewModel.Number] = viewModel.QuestionGroup;
            }

            var surveyViewModel = new SurveyViewModel(survey);

            surveyViewModel.Save(_persistenceService);

            return View("SurveyDesign", surveyViewModel);
        }

        /// <summary>
        /// Show Delete Question Group View.
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public ActionResult DeleteQuestionGroup(string surveyId, string groupId)
        {            
            var survey = _persistenceService.GetSurveys().First(item => item.ID.ToString() == surveyId);

            ViewBag.QuestionGroupId = groupId;

            var viewModel = new QuestionGroupViewModel(survey.QuestionGroups[int.Parse(groupId)])
            {
                SurveyId = surveyId,
                Number = int.Parse(groupId)
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
            var survey = _persistenceService.GetSurveys().First(item => item.ID.ToString() == viewModel.SurveyId);

            if (viewModel.QuestionGroup != null && viewModel.QuestionGroup.Questions != null)
            {
                var questionGroupToDelete = survey.QuestionGroups[viewModel.Number];
                survey.QuestionGroups.Remove(questionGroupToDelete.Number);
            }

            var surveyViewModel = new SurveyViewModel(survey);

            surveyViewModel.Save(_persistenceService);

            return View("SurveyDesign", surveyViewModel);
        }

        



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveyDesign(string id)
        {
            //Using default SurveyType of Surveillance vs Evaluation, Assessment, Audit

            Survey survey = null;
            if (!String.IsNullOrWhiteSpace(id))
            {                
                var surveys = _persistenceService.GetSurveys();                

                survey = surveys.FirstOrDefault(item => item.ID.ToString() == id);
            }
            else
            {
                survey = new Survey();
            }

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
        public ActionResult AddQuestion(string surveyId, string questionGroupId)
        {            
            var survey = _persistenceService.GetSurvey(Convert.ToInt32(surveyId));

            var questionGroup = survey.QuestionGroups[Convert.ToInt32(questionGroupId)];

            var question = questionGroup.AddQuestion(string.Empty, QuestionType.SelectOne);

            new SurveyViewModel(survey).Save(_persistenceService);

            // return View("EditQuestionGroup", new QuestionGroupViewModel(questionGroup) { SurveyId = surveyId });

            var viewModel = new QuestionViewModel(question);

            viewModel.AvailableTOCs = _standardManagementService.GetTOCs();

            return PartialView("EditorTemplates/QuestionViewModel", viewModel);
        }
                
        public ActionResult EditQuestion(string surveyId, string questionId)
        {
            var ajax = Request.IsAjaxRequest();

            var survey = _persistenceService.GetSurvey(Convert.ToInt32(surveyId));

            var id = Int32.Parse(questionId);

            var questionGroup = from a in survey.QuestionGroups
                           where a.Value.Questions.Exists(m=>m.Number == id)
                           select a;
            var question = questionGroup.First().Value.Questions.First(m => m.Number == id);
                        
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

        public PartialViewResult ShowQuestion(string surveyId, string questionGroupId, string questionId)
        {
            var survey = _persistenceService.GetSurvey(Convert.ToInt32(surveyId));

            var questionGroup = survey.QuestionGroups[Convert.ToInt32(questionGroupId)];

            var question = questionGroup.Questions.First(m => m.Number == Int32.Parse(questionId));

            var viewModel = new QuestionViewModel(question);

            viewModel.SurveyId = surveyId;
            
            return PartialView("EditorTemplates/QuestionViewModel", viewModel);
        }

        [HttpPost]
        public PartialViewResult EditQuestion(QuestionViewModel viewModel)
        {
            var survey = _persistenceService.GetSurvey(Int32.Parse(viewModel.SurveyId));

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
        public ActionResult DeleteQuestion(string surveyId, string questionGroupId, string questionId)
        {
            var survey = _persistenceService.GetSurvey(Convert.ToInt32(surveyId));

            var questionGroup = survey.QuestionGroups[Convert.ToInt32(questionGroupId)];

            var question = questionGroup.Questions.First(m => m.Number == Int32.Parse(questionId));

            questionGroup.Questions.Remove(question);

            new SurveyViewModel(survey).Save(_persistenceService);
            
            return new JsonResult { Data = "success" };
        }
	}
}