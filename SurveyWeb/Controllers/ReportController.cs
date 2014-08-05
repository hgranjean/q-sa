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
        private AtumSurveillanceContext _dbContext = null;

        public ReportController()           
        {
            _dbContext = new AtumSurveillanceContext();
        }

        //
        // GET: /Report/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProgressReport()
        {
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            var surveys = persistenceService.GetSurveys();

            // Filtering out responses by user
            var userId = User.Identity.GetUserId();
            var responses = _dbContext.Responses.Where(m => m.UserId == userId).Select(m => m.Id);

            var surveyModels = new List<TracerViewModel>();
            // var refData = new SurveillanceController();
            foreach (var response in responses)
            {
                var tracerModel = persistenceService.LoadTracer(response);
                var survey = persistenceService.GetSurvey(tracerModel.SurveyId);
                tracerModel.QuestionGroups = new QuestionGroupsViewModel(tracerModel.SurveyId.ToString(), survey.QuestionGroups);
                
                // refData.LoadTracerReferenceData(tracerModel);
                surveyModels.Add(tracerModel);
            }

            // x - elements
            // y - areas
            // value - completion
            var areas = surveyModels.First().Areas.ToList();            
            var elements = surveyModels.First().QuestionGroups.ToList();
            decimal[,] elementsByAreas = new decimal[areas.Count(), elements.Count()];
            foreach (var survey in surveyModels)
            {
                int qIndex = 0;                
                foreach (var questionGroup in survey.QuestionGroups)
                {
                    int responseCount = 0;
                    foreach (var question in questionGroup.QuestionGroup.Questions)
                    {                        
                        if (survey.Responses[qIndex] != null)
                        {
                            responseCount++;
                        }                        
                        qIndex++;
                    }
                    var completionPercent = (responseCount / qIndex) * 100;
                    
                    elementsByAreas[areas.FindIndex(area => area.ID == survey.AreaId), questionGroup.Number-1] = completionPercent; // TODO aggregate
                }                
            }

            var progressReportViewModel = new ProgressReportViewModel(elementsByAreas);
            
            return View();
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
