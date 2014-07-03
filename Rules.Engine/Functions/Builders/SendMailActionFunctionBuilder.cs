using Rules.Domain;
using Rules.Engine.Infos;

namespace Rules.Engine.Functions.Builders
{
    internal class SendMailActionFunctionBuilder : FunctionBuilder
    {
        public override FunctionBuilder GetFunctionBuilder(Rule rule, CompileContext compileContext)
        {
            var action = rule as SendMailAction;
            if (action != null)
            {
                var info = new SendMailActionInfo();
                info.Context = compileContext;
                info.FromInfo = new EvalInfo(action.From);
                info.ToInfo = new EvalInfo(action.To);
                info.SubjectInfo = new EvalInfo(action.Subject);
                info.BodyInfo = new EvalInfo(action.Body);

                return new SendMailFunction { Info = info };
            }

            return null;
        }
    }
}
