using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SurveyWeb.Filters;

namespace SurveyWeb.Controllers
{
    [System.Web.Mvc.Authorize]
    [InitializeSimpleMembership]
    public class AssessmentController : ApiController
    {
        public ActionResult Index()
        {
            return null;
            // return View();
        }
    }
}
