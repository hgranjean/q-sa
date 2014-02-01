using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;
using Rules.Engine.Infos;

namespace Rules.Engine.Functions
{
    internal class SimpleRuleSetFunction : FunctionBuilderBase
    {
        public SimpleRuleSetInfo Info { get; set; }

        public override void BuildInfo(Engine engine, CompiledBlock block, IInfo info)
        {   
            var actionInfo = (SimpleRuleSetInfo)info;

            var condition = engine.GetExpressionForValue(actionInfo.Context, actionInfo.ConditionInfo);

            var expressions = new List<Expression>();

            foreach (var childInfo in actionInfo.TargetInfo)
            {  
                var compiledBlock = engine.RuleApplicationInfo.GetCompiledBlock(new FunctionBuilder(), engine, actionInfo.Context, ((FunctionInfo)childInfo).Rule);

                expressions.Add(compiledBlock.Code);
            }

            var ifTrue = Expression.Block(expressions);

            block.Code = Expression.IfThen(condition, ifTrue);
        }
    }
}
