using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Atum.Domain.Surveillance;
using SurveyWeb;
using SurveyWeb.Models;

namespace MvcApplication1.Controllers
{
    public class AdministerAssessmentController : Controller
    {
        public ActionResult Index()
        {
            var surveys = SurveyViewModel.GetSurveys();

            return View(surveys.Select(survey => new SurveyViewModel(survey)).ToList());
        }

        public ActionResult Create(Survey survey)
        {
            var viewModel = new SurveyViewModel(survey);

            viewModel.Save();

            return View(viewModel);
        }

        public ActionResult QuestionGroupCreate(long surveyId)
        {
            var survey = SurveyServices.GetSurveys().First(item => item.ID == surveyId);

            return View(new SurveyViewModel(survey));
        }

        [HttpPost]
        public ActionResult CreateQuestionGroup(SurveyViewModel viewModel)
        {
            // var survey = SurveyServices.GetSurveys().First(item => item.ID == surveyId);

            if (viewModel.Survey.QuestionGroups == null)
            {
                viewModel.Survey.QuestionGroups = new QuestionGroups();
            }
            int newGroupIndex = viewModel.Survey.QuestionGroups.Count() + 1;
            
            viewModel.Survey.AddQuestionGroup("New Group " + newGroupIndex);

            return View("Create", viewModel);
        }

        [HttpPost]
        public ActionResult Save(SurveyViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}
