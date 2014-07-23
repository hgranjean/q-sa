using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    /// <summary>
    /// Survey Management
    /// Surveillances – Survey + Event
    /// Audits – Performing Surveillances, Surveys (Survey + Responses)
    /// </summary>
    public class SurveyController : Controller
    {
        //
        // GET: /Survey/
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Display a list of Ad-hoc Observations
        /// </summary>
        /// <returns></returns>
        public ActionResult Observations(int? ownerId)
        {
            return View();
        }

        /// <summary>
        /// Update Observation View
        /// </summary>
        /// <param name="observation"></param>
        /// <returns></returns>
        public ActionResult ClassifyObservation(string observation)
        {
            return View();
        }
	}
}