using System.ComponentModel;
using System.IO;
using Rules.WebEditor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Collections;
using Rules.Domain;

namespace Rules.WebEditor.Controllers
{
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

        internal class CustomModelBinder : DefaultModelBinder
        {
            private Type _modelType;

            protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
            {
                try
                {
                    // Retrieve custom model type to replace default model
                    string modelToCreate = controllerContext.HttpContext.Request.Params["BladeViewModel.ModelType"];

                    // Assuming viewmodels are in the same assembly as the caller, reference the type by name
                    var model = Activator.CreateInstance(this.GetType().Assembly.FullName, modelToCreate);

                    _modelType = model.Unwrap().GetType();
                }
                catch (Exception)
                { 
                    // If something goes wrong, bind to the default model creation sequence
                }

                return base.CreateModel(controllerContext, bindingContext, modelType);
            }

            protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext,
                PropertyDescriptor propertyDescriptor)
            {
                base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
            }

            protected override void SetProperty(ControllerContext controllerContext, ModelBindingContext bindingContext,
                PropertyDescriptor propertyDescriptor, object value)
            {
                var propInfo = _modelType.GetProperty(propertyDescriptor.Name);

                if (propInfo != null && propInfo.PropertyType.UnderlyingSystemType.Name == typeof(List<>).Name)
                {
                    var list = (IList)typeof(List<>)
                            .MakeGenericType(typeof(object))
                            .GetConstructor(Type.EmptyTypes)
                            .Invoke(null);

                    for (var i = 0; i < ((IList) value).Count; i++)
                    {
                        var item = Activator.CreateInstance(this.GetType().Assembly.FullName, controllerContext.RequestContext.HttpContext.Request.Params["BladeViewModel.Rules[" + i + "].ModelType"]);

                        list.Add(item.Unwrap());
                    }

                    value = list;
                }
                
                base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
            }
        }

        [HttpPost]
        [Route("BladeEdit")] // Make model specific to each bladeedit action
        public ActionResult BladeEdit(SimpleRuleSetViewModel model, FormCollection collection)
        {
            // Uses custom model binder: http://stackoverflow.com/questions/21425111/asp-net-mvc-fill-viewmodel-from-formcollection

            var modelTypeName = collection["BladeViewModel.ModelType"];

            object viewModel = Activator.CreateInstance(this.GetType().Assembly.FullName, modelTypeName);

            string @namespace = ((ObjectHandle) viewModel).Unwrap().GetType().Namespace;
            FormCollection binderCollection = new FormCollection();
            foreach (var item in collection.Keys)
            {   
                binderCollection.Add(item.ToString()
                    .Replace("BladeViewModel" + Type.Delimiter, String.Empty/*modelTypeName*/)
                    .Replace(@namespace + Type.Delimiter, String.Empty), ((string[])collection.GetValue(item.ToString()).RawValue)[0]);
            }

            if (!TryUpdateModel(model, binderCollection.ToValueProvider()))
            {
                throw new InvalidDataException("Unable to update the model.");
            }

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
