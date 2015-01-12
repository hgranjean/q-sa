using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rules.Domain;

namespace Rules.WebEditor.Models
{   
    public class ViewModel<T> where T : RuleObjectBase
    {
        public T Model { get; set; }

        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; }
        }
    }
}