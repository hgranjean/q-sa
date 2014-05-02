using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    [Authorize]
    public class SurveyDesignController : Controller
    {
        //
        // GET: /SurveyDesign/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /SurveyDesign/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /SurveyDesign/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SurveyDesign/Create

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
        // GET: /SurveyDesign/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /SurveyDesign/Edit/5

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
        // GET: /SurveyDesign/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /SurveyDesign/Delete/5

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

        //
        // GET: /SurveyDesign/Details/5

        public ActionResult Questions(int? id)
        {
            return View();
        }


        public ActionResult AddResponseChoice() 
        {
            return View();
        }

    }
}
