using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class QTrackerAdminController : Controller
    {
        // GET: QTrackerAdmin
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}