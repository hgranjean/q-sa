using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;
using Rules.Engine.Infos;

namespace Rules.Engine.Functions
{
    internal class SetValueActionFunctionBuilder : FunctionBuilder
    {
        public override FunctionBuilderBase GetFunctionBuilder(Rule rule, CompileContext compileContext)
        {
            var action = rule as SetValueAction;
            if (action != null)
            {
                var info = new SetValueActionInfo();
                info.Context = compileContext;
                info.TargetInfo = new EvalInfo(action.Target);
                info.ValueInfo = new EvalInfo(action.Value);

                return new SetValueActionFunction {Info = info};
            }

            return null;
        }
    }
}
