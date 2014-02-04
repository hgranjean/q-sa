using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SurveyWeb.Models;

namespace MvcApplication1.Controllers
{
    public class AdministerAssessmentController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(AdministeredAssessmentViewModel viewModel)
        {
            return View();
        }
    }
}
