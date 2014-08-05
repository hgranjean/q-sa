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

namespace SurveyWeb.Controllers
{
    /// <summary>
    /// Survey Management
    /// Survey Template Definitions etc...
    /// </summary>
    public class SurveyController : Controller
    {
        private AtumSurveillanceContext _dbContext = null;

        public SurveyController()
           : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
            _dbContext = new AtumSurveillanceContext();
        }

        public SurveyController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

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
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            var surveys = persistenceService.GetSurveys();            

            var model = new SurveysViewModel { Surveys = new Surveys() };
            model.Surveys.AddRange(surveys);

            ViewBag.ShowAdminContent = UserManager.IsInRole(User.Identity.GetUserId(), "Administrator");
            ViewBag.ShowSurveyorContent = UserManager.IsInRole(User.Identity.GetUserId(), "Surveyor");

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
                var persistenceService = ServiceManager.GetService<PersistenceServices>();
                var surveys = persistenceService.GetSurveys();                

                survey = surveys.FirstOrDefault(item => item.ID.ToString() == id);
            }

            var model = new SurveyViewModel(survey);

            return View(model);
        }


        public ActionResult DeleteSurvey(long id)
        {
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            var surveys = persistenceService.GetSurveys();
            
            var model = surveys.FirstOrDefault(m => m.ID == id);
            
            Contract.Requires(model != default(Survey));

            return View(model);
        }

        [HttpPost]
        public ActionResult DeleteSurvey(SurveyViewModel model)
        {
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            persistenceService.DeleteSurvey(model.Survey.ID.ToString());

            return RedirectToAction("Surveys");
        }

	}
}