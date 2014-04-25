using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rules.Domain;
using Rules.WebEditor.Models.Actions;

namespace Rules.WebEditor.Models
{
    public class SimpleRuleSetViewModel
    {
        private readonly SimpleRuleSet _model;

        public SimpleRuleSetViewModel(SimpleRuleSet model)
        {
            _model = model;
        }

        public Expression Condition { get { return new Expression(_model.Condition); } }

        public List<object> Rules { get { return _model.Rules.ConvertAll(m => ViewModelConverter.Convert(m)); } }
    }

    public class ViewModelConverter
    {
        // Returns view model
        public static object Convert(object domainModel)
        {
            if (domainModel is SimpleRuleSet)
            {
                return new SimpleRuleSetViewModel(domainModel as SimpleRuleSet);
            } else if (domainModel is SetValueAction)
            {
                return new SetValueActionViewModel(domainModel as SetValueAction);
            }
            
            throw new NotSupportedException("ViewModel is unhandled for the domainModel");
        }
    }
}