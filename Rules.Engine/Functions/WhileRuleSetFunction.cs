using Rules.Engine.Infos;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rules.Engine.Functions
{
    internal class WhileRuleSetFunction : FunctionBuilder
    {
        public WhileRuleSetInfo Info { get; set; }

        public override void BuildInfo(Engine engine, CompiledBlock block, IInfo info)
        {
            var actionInfo = (WhileRuleSetInfo)info;

            var condition = engine.GetExpressionForValue(actionInfo.Context, actionInfo.ConditionInfo);

            var expressions = new List<Expression>();

            foreach (var childInfo in actionInfo.TargetInfo)
            {
                var compiledBlock = engine.RuleApplicationInfo.GetCompiledBlock(new FunctionInfo(), engine, actionInfo.Context, ((Infos.FunctionInfo)childInfo).Rule);

                expressions.Add(compiledBlock.Code);
            }

            if (expressions.Count == 0)
            {
                expressions.Add(Expression.Empty());
            }

            var ifTrue = Expression.Block(expressions);

            var breakLabel = Expression.Label("LoopBreakLabel");

            block.Code = Expression.Block(Expression.Loop(Expression.IfThenElse(Expression.Convert(condition, typeof(bool)), ifTrue, Expression.Goto(breakLabel))),
                Expression.Label(breakLabel));
        }
    }
}
