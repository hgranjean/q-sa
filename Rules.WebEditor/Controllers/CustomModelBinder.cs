using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Rules.WebEditor.Controllers
{
    internal class CustomModelBinder : DefaultModelBinder
    {
        private Type _modelType;
        private IList _customProperties;
        private IValueProvider _valueProvider;

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            try
            {
                // Retrieve custom model type to replace default model
                string modelToCreate = controllerContext.HttpContext.Request.Params["BladeViewModel.ModelType"];

                // Assuming viewmodels are in the same assembly as the caller, reference the type by name
                var model = Activator.CreateInstance(this.GetType().Assembly.FullName, modelToCreate);

                _modelType = model.Unwrap().GetType();
                _valueProvider = bindingContext.ValueProvider;
            }
            catch (Exception)
            { 
                // If something goes wrong, bind to the default model creation sequence
            }

            return base.CreateModel(controllerContext, bindingContext, modelType);
        }

            
        protected virtual void GetCustomModel(ControllerContext controllerContext, object value)
        {
            var list = (IList) typeof (List<>)
                .MakeGenericType(typeof (object))
                .GetConstructor(Type.EmptyTypes)
                .Invoke(null);

            for (var i = 0; i < ((IList) value).Count; i++)
            {
                var item = Activator.CreateInstance(this.GetType().Assembly.FullName,
                    controllerContext.RequestContext.HttpContext.Request.Params["BladeViewModel.Rules[" + i + "].ModelType"]);
                    
                list.Add(item.Unwrap());
            }

            _customProperties = list;
        }

        protected virtual object GetCustomModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext, int index)
        {
            var item = Activator.CreateInstance(this.GetType().Assembly.FullName,
                controllerContext.RequestContext.HttpContext.Request.Params["BladeViewModel.Rules[" + index + "].ModelType"]);

            return item.Unwrap();
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {   
            if (bindingContext != null && bindingContext.ModelName != null && bindingContext.ModelName.StartsWith("BladeViewModel.Rules[") && bindingContext.ModelName.EndsWith("]"))
            {
                var parts = bindingContext.ModelName.Split(new[] { '[', ']' });
                var index = Convert.ToInt32(parts[1]);

                var model = GetCustomModel(controllerContext, bindingContext, index);
                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());
            }

            return base.BindModel(controllerContext, bindingContext);
        }

        internal ModelBindingContext CreateComplexElementalModelBindingContext(ControllerContext controllerContext, ModelBindingContext bindingContext, object model)
        {
            BindAttribute bindAttr = (BindAttribute)GetTypeDescriptor(controllerContext, bindingContext).GetAttributes()[typeof(BindAttribute)];
            Predicate<string> newPropertyFilter = (bindAttr != null)
                ? propertyName => bindAttr.IsPropertyAllowed(propertyName) && bindingContext.PropertyFilter(propertyName)
                : bindingContext.PropertyFilter;

            ModelBindingContext newBindingContext = new ModelBindingContext()
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, bindingContext.ModelType),
                ModelName = bindingContext.ModelName,
                ModelState = bindingContext.ModelState,
                PropertyFilter = newPropertyFilter,
                ValueProvider = bindingContext.ValueProvider
            };

            return newBindingContext;
        }
    }
}