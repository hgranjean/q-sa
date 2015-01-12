using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rules.WebEditor.Models
{
    public class Expression
    {
        private string _expression;

        public Expression() { }
        
        public Expression(string expression)
        {
            _expression = expression ?? String.Empty;
        }

        public string ExpressionValue
        {
            get { return _expression; }
            set { _expression = value;  }
        }

        internal static string ToString(Expression value)
        {
            return value.ExpressionValue;
        }
    }
}