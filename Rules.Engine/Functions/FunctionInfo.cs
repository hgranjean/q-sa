using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;
using Rules.Engine.Functions.Builders;
using Rules.Engine.Infos;

namespace Rules.Engine.Functions
{
    internal class FunctionInfo
    {
        public virtual FunctionBuilder GetFunctionBuilder(Rule rule, CompileContext compileContext)
        {
            if (rule is SetValueAction)
            {
                return new SetValueActionFunctionBuilder();
            }
            else if (rule is SimpleRuleSet)
            {
                return new SimpleRuleSetFunctionBuilder();
            }

            throw new Exception("unknown func builder");
        }
    }
}
