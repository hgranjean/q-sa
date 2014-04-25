using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;

namespace Rules.Engine.Base
{
    internal class RuleSetInfo : InfoBase
    {
        public RuleSpec RuleSpec { get; set; }
        public LambdaExpression Lambda { get; set; }
    }
}
