using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rules.Engine.Infos;
using Rules.Engine.Infos.Templates;

namespace Rules.Engine.Functions.Templates
{
    internal class ExpressionTemplateFunction : FunctionBuilder
    {
        public TemplateInfo Info { get; set; }

        public override void BuildInfo(Engine engine, CompiledBlock block, IInfo info)
        {
            var actionInfo = (TemplateInfo) info;
            
            if (block.Variables == null)
            {
                block.Variables = new List<ParameterExpression>();
            }
            
            // TODO: Implement parameterized templates

            // block.Variables.Add(varExpr);

            if (actionInfo.ValueInfo != null)
            {
                block.Code = engine.GetExpressionForValue(actionInfo.Context, actionInfo.ValueInfo);
            }
        }
    }
}
