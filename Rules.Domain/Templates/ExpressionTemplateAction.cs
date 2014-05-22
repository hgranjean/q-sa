using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain.Vocabulary;

namespace Rules.Domain.Templates
{
    public class ExpressionTemplateAction : RuleAction
    {
        public new String Name { get { return base.Name; } set { base.Name = value; } }
        public String Value { get; set; }
        public String ValueType { get; set; }

        public ExpressionTemplateAction(TemplateSpec templateSpec)
        {
            this.Name = templateSpec.Name;
            this.Value = templateSpec.Expression;
            this.ValueType = typeof (bool).ToString(); // TODO: Only bool is supported now.
        }
    }
}
