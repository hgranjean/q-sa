using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rules.Domain;
using Rules.WebEditor.Models.Actions;
using System.Diagnostics.Contracts;

namespace Rules.WebEditor.Models
{
    public class SimpleRuleSetViewModel
    {
        private readonly SimpleRuleSet _model;

        public SimpleRuleSetViewModel()
        {
            _model = new SimpleRuleSet();
        }

        public SimpleRuleSetViewModel(SimpleRuleSet model)
        {
            _model = model;
        }

        public Expression Condition
        {
            get { return new Expression(_model.Condition); }
            set { _model.Condition = value.ExpressionValue; }
        }

        public List<object> Rules {
            get { return _model.Rules.ConvertAll(m => ViewModelConverter.Convert(m)); }
            set { _model.Rules = value.ConvertAll(m => ViewModelConverter.ConvertFrom(m)); }
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
            }

            throw new InvalidCastException("Unable to convert object into rule." + model.GetType());
        }
    }
}