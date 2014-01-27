using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Web.Mvc;
using System.Web.UI.WebControls;
using SurveyWeb.Filters;

namespace SurveyWeb.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AssessmentController : Controller
    {
        public ActionResult Index()
        {   
            return View();
        }
    }
}
