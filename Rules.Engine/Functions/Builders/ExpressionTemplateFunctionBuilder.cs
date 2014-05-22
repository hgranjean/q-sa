using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;
using Rules.Domain.Templates;
using Rules.Engine.Functions.Templates;
using Rules.Engine.Infos.Templates;

namespace Rules.Engine.Functions.Builders
{
    internal class ExpressionTemplateFunctionBuilder : FunctionBuilder
    {
        public override FunctionBuilder GetFunctionBuilder(Rule rule, CompileContext compileContext)
        {
            var action = rule as ExpressionTemplateAction;
            if (action != null)
            {
                var info = new TemplateInfo();
                info.Context = compileContext;
                info.ValueInfo = new EvalInfo(action.Value);
                info.ValueType = new DataTypeInfo(action.ValueType);

                return new ExpressionTemplateFunction { Info = info };
            }

            return null;
        }
    }
}
