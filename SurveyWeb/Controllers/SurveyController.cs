using Atum.Domain.Common;
using Atum.Domain.SurveyManagement;
using Atum.Utility.XML;
using SurveyWeb.Models;
using SurveyWeb.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    /// <summary>
    /// Survey Management
    /// Survey Template Definitions etc...
    /// </summary>
    public class SurveyController : Controller
    {
        //
        // GET: /Survey/
        public ActionResult Index()
        {
            return View();
        }


        
        /// <summary>
        /// Show New Survey Screen
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
        public ActionResult Create(Survey survey)
        {
            var viewModel = new SurveyViewModel(survey);

            viewModel.Save();

            return View(viewModel);
        }

        public ActionResult CreateQuestionGroup(long surveyId)
        {
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            var survey = persistenceService.GetSurveys().First(item => item.ID == surveyId);

            return View(new SurveyViewModel(survey));
        }

        [HttpPost]
        public ActionResult CreateQuestionGroup(SurveyViewModel viewModel)
        {
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            var survey = persistenceService.GetSurvey(Convert.ToInt32(viewModel.Survey.ID));

            if (survey.QuestionGroups == null)
            {
                survey.QuestionGroups = new QuestionGroups();
            }
            int newGroupIndex = survey.QuestionGroups.Count() + 1;

            survey.AddQuestionGroup("New Group " + newGroupIndex);

            viewModel = new SurveyViewModel(survey);

            viewModel.Save();

            return View("SurveyDesign", viewModel);
        }

        /// <summary>
        /// TODO: Move method to Survey Controller.  Move Save Execution to Services
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(SurveyViewModel viewModel)
        {
            //if (viewModel.Survey.Guid == Guid.Empty)
            //{
            //    viewModel.Survey.Guid = Guid.NewGuid();

            //    _dbContext.Surveys.Add(new SurveyEntry
            //    {
            //        Id = viewModel.Survey.Guid.ToString("d"),
            //        Title = viewModel.Survey.Title
            //    });
            //}
            //else
            //{
            //    var id = viewModel.Survey.Guid.ToString("d");
            //    var surveyEntry = _dbContext.Surveys.FirstOrDefault(m => m.Id == id);

            //    surveyEntry.Title = viewModel.Survey.Title;
            //}

            //_dbContext.SaveChanges();

            //viewModel.Save();

            return View();
        }
        /// <summary>
        /// View Edit Question Group Screen/Page
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public ActionResult EditQuestionGroup(string surveyId, string groupId)
        {
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            var survey = persistenceService.GetSurveys().First(item => item.ID.ToString() == surveyId);

            ViewBag.QuestionGroupId = groupId;

            return View(new QuestionGroupViewModel(survey.QuestionGroups[int.Parse(groupId)])
            {
                SurveyId = surveyId,
                Number = int.Parse(groupId)
            });
        }


        /// <summary>
        /// Update an Edited Question Group
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditQuestionGroup(QuestionGroupViewModel viewModel)
        {
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            var survey = persistenceService.GetSurveys().First(item => item.ID.ToString() == viewModel.SurveyId);

            if (viewModel.QuestionGroup != null && viewModel.QuestionGroup.Questions != null)
            {
                survey.QuestionGroups[viewModel.Number] = viewModel.QuestionGroup;
            }

            var surveyViewModel = new SurveyViewModel(survey);

            surveyViewModel.Save();

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
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            var survey = persistenceService.GetSurveys().First(item => item.ID.ToString() == surveyId);

            ViewBag.QuestionGroupId = groupId;

            return View(new QuestionGroupViewModel(survey.QuestionGroups[int.Parse(groupId)])
            {
                SurveyId = surveyId,
                Number = int.Parse(groupId)
            });
        }
        /// <summary>
        /// Delete a Question Group from a Survey 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteQuestionGroup(QuestionGroupViewModel viewModel)
        {
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            var survey = persistenceService.GetSurveys().First(item => item.ID.ToString() == viewModel.SurveyId);

            if (viewModel.QuestionGroup != null && viewModel.QuestionGroup.Questions != null)
            {
                var questionGroupToDelete = survey.QuestionGroups[viewModel.Number];
                survey.QuestionGroups.Remove(questionGroupToDelete.Number);
            }

            var surveyViewModel = new SurveyViewModel(survey);

            surveyViewModel.Save();

            return View("SurveyDesign", surveyViewModel);
        }

        /// <summary>
        /// Add a Question to a Survey
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="questionGroupId"></param>
        /// <returns></returns>
        public ActionResult AddQuestion(string surveyId, string questionGroupId)
        {
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            var survey = persistenceService.GetSurvey(Convert.ToInt32(surveyId));

            var questionGroup = survey.QuestionGroups[Convert.ToInt32(questionGroupId)];

            questionGroup.AddQuestion(string.Empty, QuestionType.SelectOne);

            new SurveyViewModel(survey).Save();

            return View("EditQuestionGroup", new QuestionGroupViewModel(questionGroup) { SurveyId = surveyId });
        }

        public ActionResult EditNotes(string questionId)
        {
            return View();
        }


	}
}