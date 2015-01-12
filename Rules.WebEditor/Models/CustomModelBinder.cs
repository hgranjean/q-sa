using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rules.WebEditor.Models
{
    /*
    public class StatementModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var statementTypeParameter = bindingContext.ValueProvider.GetValue("StatementType");
            if (statementTypeParameter == null)
                throw new InvalidOperationException("StatementType is not specified");

            StatementType statementType;
            if (!Enum.TryParse(statementTypeParameter.AttemptedValue, true, out statementType))
                throw new InvalidOperationException("Incorrect StatementType"); // not sure about the type of exception

            var model = SomeFactoryHelper.GetStatementByType(statementType); // returns an actual model by StatementType parameter
            // this could be a simple switch statement
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, model.GetType());
            bindingContext.ModelMetadata.Model = model;
            return model;
        }
    }
     * 
     * After that register the model binder in the Global.asax:

ModelBinders.Binders.Add(typeof(StatementViewModel), new StatementModelBinder());
     * 
     * 
     * public class StatementVMBinder : DefaultModelBinder
{
    // this is the only method you need to override:
    protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
    {
        if (modelType == typeof(StatementViewModel)) // so it will leave the other VM to the default implementation.
        {
            // this gets the value from the form collection, if it was in an input named "ViewModelName":
            var discriminator = bindingContext.ValueProvider.GetValue("ViewModelName");
            Type instantiationType;
            if (discriminator == "SomethingSomething")
                instantiationType = typeof(ReliefVM);
            else // or do a switch case
                instantiationType = typeof(RequestForSalaryVM);

            var obj = Activator.CreateInstance(instantiationType);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, instantiationType);
            bindingContext.ModelMetadata.Model = obj;
            return obj;
        }
        return base.CreateModel(controllerContext, bindingContext, modelType);
    }
}
    */


}