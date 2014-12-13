using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rules.Domain;

namespace Rules.WebEditor.Models.Actions
{
    public class SetValueActionViewModel : ViewModel<SetValueAction>
    {
        public SetValueActionViewModel()
        {
            this.Model = new SetValueAction();
        }

        public SetValueActionViewModel(SetValueAction model)
        {
            this.Model = model;
        }

        public Expression Target { get { return new Expression(Model.Target); } set { Model.Target = Expression.ToString(value); } }
        public Expression Value { get { return new Expression(Model.Value); } set { Model.Value = Expression.ToString(value); } }
    }
}