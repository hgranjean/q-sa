using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;
using Rules.Domain.Templates;
using Rules.Engine.Functions.Templates;
using Rules.Engine.Infos;
using Rules.Engine.Infos.Templates;

namespace Rules.Engine.Functions.Builders
{
    internal class FunctionNodeFunctionBuilder : FunctionBuilder
    {
        public override FunctionBuilder GetFunctionBuilder(Rule rule, CompileContext compileContext)
        {
            var action = rule as FunctionNode;
            if (action != null)
            {
                var info = new Infos.FunctionInfo(rule);
                // info.Context = compileContext;
                // info.ValueInfo = new EvalInfo(action.Expression);
                // info.ValueType = new DataTypeInfo(action.ValueType);

                return new FunctionNodeFunction { Info = info };
            }

            return null;
        }
    }
}
