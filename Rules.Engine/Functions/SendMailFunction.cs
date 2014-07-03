using System;
using System.Net.Mail;
using Rules.Engine.Infos;
using System.Linq.Expressions;

namespace Rules.Engine.Functions
{
    internal class SendMailFunction : FunctionBuilder
    {
        public SendMailActionInfo Info { get; set; }

        public override void BuildInfo(Engine engine, CompiledBlock block, IInfo info)
        {
            var actionInfo = (SendMailActionInfo)info;

            var lhs = engine.GetExpressionForValue(actionInfo.Context, actionInfo.FromInfo);
            Type type = null;

            if (lhs is ParameterExpression)
            {
                type = lhs.Type;
            }
            else if (lhs is IndexExpression)
            {
                type = typeof(object);
            }
            else if (lhs is UnaryExpression)
            {
                type = ((UnaryExpression)lhs).Operand.Type;
            }
            else
            {
                type = lhs.Type;
            }

            var rhs = Expression.Convert(engine.GetExpressionForValue(actionInfo.Context, actionInfo.FromInfo), type);

            // block.Code = Expression.Assign(lhs, rhs);

            var client = Expression.New(typeof (SmtpClient));
            
            var message = Expression.New(typeof (MailMessage));

            block.Code = Expression.Call(client, "Send", null, message);
        }
    }
}
