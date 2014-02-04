using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;
using Rules.Engine.Infos;

namespace Rules.Engine
{
    internal class SetValueActionFunction : FunctionBuilder
    {
        public SetValueActionInfo Info { get; set; }

        public override void BuildInfo(Engine engine, CompiledBlock block, IInfo info)
        {
            var actionInfo = (SetValueActionInfo) info;
            
            block.Code = Expression.Assign(engine.GetExpressionForValue(actionInfo.Context, actionInfo.TargetInfo, true),
                                           engine.GetExpressionForValue(actionInfo.Context, actionInfo.ValueInfo));
        }
    }
}
