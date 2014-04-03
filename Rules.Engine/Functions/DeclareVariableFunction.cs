using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
                var primitiveValueExpr = ConvertToPrimitiveType(engine
                                            .GetExpressionForValue(actionInfo.Context, actionInfo.ValueInfo), actionInfo.ValueType);
                
                var defaultValExpr = Expression.Assign(varExpr, primitiveValueExpr);

                block.Code = defaultValExpr;
            }
        }

        private Expression ConvertToPrimitiveType(Expression valueExpression, IDataTypeInfo targetType)
        {
            if (targetType.SystemType == typeof (string))
            {
                return Expression.Call(
                    Expression.Convert(valueExpression, typeof (object)),
                    typeof (object).GetMethod("ToString"));
            }
            if (targetType.SystemType == typeof (int))
            {
                return Expression.Call(
                    typeof(Convert).GetMethod("ToInt32", new[] { typeof(object) }), valueExpression);
            }

            return valueExpression;
        }
    }
}
