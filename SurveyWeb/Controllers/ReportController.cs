using System.Data.Entity;
using System.Data.Entity.Validation;
using Atum.Database.Surveillance.Models;
using Atum.Domain;
using Atum.Domain.Common;
using Atum.Domain.Healthcare;
using Atum.Domain.Security.Domain;
using Atum.Domain.SurveyManagement;
using Atum.Utility.XML;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SurveyWeb.Models;
using SurveyWeb.RuleApp;
using SurveyWeb.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Atum.Utility;

namespace SurveyWeb.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IPersistenceServices _persistenceService;
        private readonly SurveillanceService _surveillanceService;
        private readonly ReportService _reportService;

        public ReportController(IPersistenceServices persistenceService, SurveillanceService surveillanceService, ReportService reportService)           
        {
            _persistenceService = persistenceService;
            _surveillanceService = surveillanceService;
            _reportService = reportService;
        }

        //
        // GET: /Report/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProgressReport()
        {        
            var userId = User.Identity.GetUserId();
            
            var model = _reportService.GetProgressReport(userId);

            var viewModel = new ProgressReportViewModel(model);

            return View(viewModel);
        }
        
        public ActionResult CompletionReport()
        {
            return View();
        }

        public ActionResult PersonPerformance()
        {
            return View();
        }

        //
        // GET: /Report/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Report/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Report/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Report/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Report/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Report/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Report/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
