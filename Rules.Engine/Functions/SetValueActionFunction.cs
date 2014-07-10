using System;
using Rules.Engine.Infos;
using System.Linq.Expressions;

namespace Rules.Engine
{
    internal class SetValueActionFunction : FunctionBuilder
    {
        public SetValueActionInfo Info { get; set; }

        public override void BuildInfo(Engine engine, CompiledBlock block, IInfo info)
        {
            var actionInfo = (SetValueActionInfo) info;

            var lhs = engine.GetExpressionForValue(actionInfo.Context, actionInfo.TargetInfo);
            
            var type = engine.GetType(lhs);

            var rhs = Expression.Convert(engine.GetExpressionForValue(actionInfo.Context, actionInfo.ValueInfo), type);

            block.Code = Expression.Assign(lhs, rhs);
        }
    }
}
