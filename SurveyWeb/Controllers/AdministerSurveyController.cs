using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using Atum.Domain.QualityManagement;
using SurveyWeb;
using SurveyWeb.Models;
using SurveyWeb.Services;
using Atum.Domain.SurveyManagement;

namespace SurveyWeb
{
    [Authorize]
    public class AdministerSurveyController : Controller
    {
        public ActionResult Index()
        {
            var surveys = SurveyViewModel.GetSurveys();

            return View(surveys.Select(survey => new SurveyViewModel(survey)).ToList());
        }

        public ActionResult Create(Survey survey)
        {
            
            if (Session["Survey"] != null)
            {
                if (TempData["IsRedirect"] != null)
                {
                    return View(Session["Survey"]);
                }
            }
            
            var viewModel = new SurveyViewModel(survey);

            Session["Survey"] = viewModel;
            
            return View(viewModel);
        }

        public ActionResult CreateQuestionGroup(long surveyId)
        {
            Contract.Requires(Session["Survey"] != null, "Survey is empty. Your session is expired and should be refreshed.");

            var survey = (SurveyViewModel) Session["Survey"];

            return View();
        }

        [HttpPost]
        public ActionResult CreateQuestionGroupComplete()
        {
            var viewModel = (SurveyViewModel) Session["Survey"];

            viewModel.AddQuestionGroup();

            Session["Survey"] = viewModel;

            TempData["IsRedirect"] = true;

            return RedirectToAction("Create");
        }

        [HttpPost]
        public ActionResult Save(SurveyViewModel survey)
        {
            var viewModel = (SurveyViewModel) Session["Survey"];

            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            persistenceService.SaveSurvey(viewModel.Survey);

            return View(viewModel);
        }
    }
}
