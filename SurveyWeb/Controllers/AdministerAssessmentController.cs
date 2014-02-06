using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Atum.Domain.Surveillance;
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

            return View();
        }

        [HttpPost]
        public ActionResult Save(AdministeredAssessmentViewModel viewModel)
        {
            return View();
        }
    }
}
