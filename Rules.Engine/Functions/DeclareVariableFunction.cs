using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rules.Engine.Infos;

namespace Rules.Engine.Functions
{
    internal class DeclareVariableFunction : FunctionBuilder
    {
        public DeclareVariableActionInfo Info { get; set; }

        public override void BuildInfo(Engine engine, CompiledBlock block, IInfo info)
        {
            var actionInfo = (DeclareVariableActionInfo) info;
            
            var varExpr = Expression.Variable(actionInfo.ValueType.SystemType, actionInfo.VariableName);

            if (block.Variables == null)
            {
                block.Variables = new List<ParameterExpression>();
            }
            
            block.Variables.Add(varExpr);

            if (actionInfo.ValueInfo != null)
            {
                var defaultValExpr = Expression.Assign(varExpr,
                                                       engine.GetExpressionForValue(actionInfo.Context,
                                                                                    actionInfo.ValueInfo));

                block.Code = defaultValExpr;
            }
        }
    }
}
