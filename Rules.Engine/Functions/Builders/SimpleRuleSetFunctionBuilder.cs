using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;
using Rules.Engine.Infos;

namespace Rules.Engine.Functions.Builders
{
    internal class SimpleRuleSetFunctionBuilder : FunctionBuilder
    {
        internal class FunctionBuilder
        {
            public virtual FunctionBuilderBase GetFunctionBuilder(Rule rule, CompileContext compileContext)
            {
                var conditional = rule as SimpleRuleSet;
                if (conditional != null)
                {
                    var info = new SimpleRuleSetInfo();
                    info.Context = compileContext;
                    info.ConditionInfo = new EvalInfo(conditional.Condition);

                    foreach (var subRule in conditional.Rules)
                    {
                        info.TargetInfo.Add(new FunctionInfo(subRule));
                    }

                    return new SimpleRuleSetFunction { Info = info };

                }

                return null;
            }
        }
    }
}
