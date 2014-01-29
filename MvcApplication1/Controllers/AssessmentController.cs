using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Web.Mvc;
using System.Web.UI.WebControls;
using MvcApplication1.Filters;
using MvcApplication1.Models;
using MvcApplication1.Services;

namespace MvcApplication1.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AssessmentController : Controller
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
            viewModel.Enumerator.MoveNextManager(viewModel.CurrentAssessment.ConductedSurvey);

            SessionBag.Current.Enumerator = viewModel.Enumerator;

            ModelState.Clear(); // refresh state

            return View("Create", viewModel);
        }

        public ActionResult Details(int assessmentId)
        {
            return View();
        }
    }
}
