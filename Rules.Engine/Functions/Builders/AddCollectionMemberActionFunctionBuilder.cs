using Rules.Domain;
using Rules.Engine.Infos;

namespace Rules.Engine.Functions.Builders
{
    internal class AddCollectionMemberActionFunctionBuilder : FunctionBuilder
    {
        public override FunctionBuilder GetFunctionBuilder(Rule rule, CompileContext compileContext)
        {
            var action = rule as AddCollectionMemberAction;
            if (action != null)
            {
                var info = new AddCollectionMemberActionInfo();
                info.Context = compileContext;
                info.TargetInfo = new EvalInfo(action.Target);

                return new AddCollectionMemberFunction { Info = info };
            }

            return null;
        }
    }
}
