using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;
using Rules.Engine.Infos;

namespace Rules.Engine.Functions.Builders
{
    internal class DeclareVariableFunctionBuilder : FunctionBuilder
    {
        public override FunctionBuilder GetFunctionBuilder(Rule rule, CompileContext compileContext)
        {
            var action = rule as DeclareVariableAction;
            if (action != null)
            {
                var info = new DeclareVariableActionInfo();
                info.Context = compileContext;
                info.ValueInfo = new EvalInfo(action.Value);
                info.VariableName = action.Name;
                info.ValueType = new DataTypeInfo(action.ValueType);

                return new DeclareVariableFunction { Info = info };
            }

            return null;
        }
    }
}
