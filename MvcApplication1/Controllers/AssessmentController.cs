using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Web.Mvc;
using System.Web.UI.WebControls;
using MvcApplication1.Filters;
using MvcApplication1.Models;

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
            return View();
        }

        [HttpPost]
        public ActionResult Save(AssessmentViewModel viewModel)
        {
            return View("Details");
        }

        public ActionResult Details(int assessmentId)
        {
            return View();
        }
    }
}
