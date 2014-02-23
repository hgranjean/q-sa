using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Web.Mvc;
using System.Web.UI.WebControls;
using Atum.Domain.Surveillance;
using SurveyWeb.Filters;
using SurveyWeb.Models;
using SurveyWeb.Services;

namespace SurveyWeb.Controllers
{
    // [Authorize]
    // [InitializeSimpleMembership]
    public class SurveyController : Controller
    {
        public ActionResult Index()
        {   
            return View();
        }

        public ActionResult Create()
        {
            var viewModel = new AssessmentViewModel();

            // TODO: Reset to the first question
            
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Save(AssessmentViewModel viewModel)
        {
            var question = viewModel.Enumerator.Current;

            viewModel.CurrentAssessment.Responses.Add(new Response(question, new ResponseChoice("1")));

            bool hasNext = viewModel.Enumerator.MoveNextManager(viewModel.CurrentAssessment.ConductedSurvey);

            SessionBag.Current.Enumerator = viewModel.Enumerator;

            ModelState.Clear(); // refresh state

            if (!hasNext)
            {
                return View("AssessmentComplete", viewModel);
            }

            return View("Create", viewModel);
        }

        public ActionResult Details(int assessmentId)
        {
            return View();
        }

        public ActionResult AssessmentComplete(AssessmentViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}
