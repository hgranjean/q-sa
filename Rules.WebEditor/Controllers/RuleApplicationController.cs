using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Rules.WebEditor.Controllers
{
    public class RuleApplicationController : Controller
    {
        //
        // GET: /RuleApplication/

        public ActionResult Index()
        {
            var ruleapps = PersistenceServices.GetRuleApplications();
            
            return View(ruleapps);
        }

        //
        // GET: /RuleApplication/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /RuleApplication/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /RuleApplication/Create

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
        // GET: /RuleApplication/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /RuleApplication/Edit/5

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
        // GET: /RuleApplication/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /RuleApplication/Delete/5

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

        public JsonResult GetMyData()
        {
            var menu = new[] { "Cut", "Copy", "Paste" };

            // new {title = "Cut", cmd = "cut", uiIcon = "ui-icon-scissors"}
            // Menu s = new SomeClass();
            // s.Property1 = "value";
            // s.Property2 = "another value";

            var str = new JavaScriptSerializer().Serialize(menu); // "[\"Cut\", \"Copy\", \"Parse\"]"; 
            // return Json(s, JsonRequestBehavior.AllowGet); 


            return Json(str, JsonRequestBehavior.AllowGet); // need the AllowGet option to return data to a GET request
        }
    }
}
