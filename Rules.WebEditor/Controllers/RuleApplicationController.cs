using System.ComponentModel;
using System.IO;
using Rules.WebEditor.Models;
using System;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using Rules.Domain;
using System.Collections.Generic;

namespace Rules.WebEditor.Controllers
{
    public static class ListExtensions
    {
        public static void ReplaceItem<T>(this List<T> list, T itemToReplace, T replacingItem)
        {
            var index = list.IndexOf(itemToReplace);
            list.Remove(itemToReplace);
            list.Insert(index, replacingItem);
        }
    }
    [RouteArea("RuleApplication")]
    [RoutePrefix("")]
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

        [HttpPost]
        [Route("BladeEdit")] // Make model specific to each bladeedit action
        public ActionResult BladeEdit(SimpleRuleSetViewModel model, FormCollection collection)
        {
            // Uses custom model binder: http://stackoverflow.com/questions/21425111/asp-net-mvc-fill-viewmodel-from-formcollection

            var prefix = typeof(BladeViewModel).Name; /* wrapper modelTypeName*/
            var modelTypeName = collection[prefix + Type.Delimiter + "ModelType"];

            // object viewModel = Activator.CreateInstance(this.GetType().Assembly.FullName, modelTypeName);
            
            FormCollection binderCollection = new FormCollection();
            foreach (var item in collection.Keys)
            {   
                binderCollection.Add(item.ToString(), ((string[])collection.GetValue(item.ToString()).RawValue)[0]);
            }

            if (!TryUpdateModel(model, prefix, binderCollection.ToValueProvider()))
            {
                throw new InvalidDataException("Unable to update the model.");
            }

            var ruleapp1 = PersistenceServices.GetRuleApplications().ToList<RuleObjectBase>();
            var existing =
                ((RuleApplicationSpec) ruleapp1.FirstOrDefault()).Entities[0].RuleSets[0].Actions[0] as SimpleRuleSet;

            ((RuleApplicationSpec)ruleapp1.FirstOrDefault()).Entities[0].RuleSets[0].Actions.ReplaceItem(existing,
                (SimpleRuleSet)ViewModelConverter.ConvertFrom(model));

            return Redirect("~/Home/Save");
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
    }
}
