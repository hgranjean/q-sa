using SurveyWeb.Models;
using SurveyWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class LearningController : Controller
    {



        public ActionResult ObservationClassifier(string observation)
        {
            //View will contain Classification and list of EP Choices
            StandardElementViewModel model = new StandardElementViewModel();
            if (observation!=null&&observation.Length>0)
            {
                model.Observation = observation;
                model = LearningServices.Classify(observation);
                    
            }

            return View(model);
        }
        
        
        //
        // GET: /Learning/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Learning/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Learning/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Learning/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Learning/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Learning/Edit/5
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
        // GET: /Learning/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Learning/Delete/5
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
