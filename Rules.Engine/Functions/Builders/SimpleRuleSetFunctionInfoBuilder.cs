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
        public override FunctionBuilder GetFunctionBuilder(Rule rule, CompileContext compileContext)
        {
            var conditional = rule as SimpleRuleSet;
            if (conditional != null)
            {
                var info = new SimpleRuleSetInfo();
                info.Context = compileContext;
                info.ConditionInfo = new EvalInfo(conditional.Condition);
                info.TargetInfo = new List<IInfo>();

                foreach (var subRule in conditional.Rules)
                {
                    info.TargetInfo.Add(new Infos.FunctionInfo(subRule));
                }

                return new SimpleRuleSetFunction { Info = info };

            }

            return null;
        }
    
    }
}
