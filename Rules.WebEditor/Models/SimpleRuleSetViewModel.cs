using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rules.Domain;
using Rules.WebEditor.Models.Actions;
using System.Diagnostics.Contracts;

namespace Rules.WebEditor.Models
{
    public class SimpleRuleSetViewModel : ViewModel<SimpleRuleSet>
    {
        public SimpleRuleSetViewModel()
        {
            Model = new SimpleRuleSet();
        }

        public SimpleRuleSetViewModel(SimpleRuleSet model)
        {
            Model = model;
        }

        public Expression Condition
        {
            get { return new Expression(Model.Condition); }
            set { Model.Condition = value.ExpressionValue; }
        }

        public List<object> Rules {
            get { return Model.Rules.ConvertAll(m => ViewModelConverter.Convert(m)); }
            set { Model.Rules = value.ConvertAll(m => ViewModelConverter.ConvertFrom(m)); }
        }
    }

    internal class ViewModelConverter
    {
        // Returns view model
        public static object Convert<T>(T domainModel)
        {
            if (domainModel is SimpleRuleSet)
            {
                return new SimpleRuleSetViewModel(domainModel as SimpleRuleSet);
            } else if (domainModel is SetValueAction)
            {
                return new SetValueActionViewModel(domainModel as SetValueAction);
            } else if (domainModel is SendMailAction)
            {
                return new SendMailActionViewModel(domainModel as SendMailAction);
            }
            
            throw new NotSupportedException("ViewModel is unhandled for the domainModel." + domainModel.GetType());
        }

        public static Rule ConvertFrom(object model)
        {
            Contract.Requires(model != null);

            if (model is SetValueActionViewModel)
            {
                return ((SetValueActionViewModel)model).Model;
            }
            else if (model is SendMailActionViewModel)
            {
                return ((SendMailActionViewModel)model).Model;
            } else if (model is SimpleRuleSetViewModel)
            {
                return ((SimpleRuleSetViewModel)model).Model;
            }

            throw new InvalidCastException("Unable to convert object into rule." + model.GetType());
        }
    }
}