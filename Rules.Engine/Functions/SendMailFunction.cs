using System;
using System.Configuration;
using System.Net.Configuration;
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

            var fromExpr = engine.GetExpressionForValue(actionInfo.Context, actionInfo.FromInfo);
            
            var toExpr = engine.GetExpressionForValue(actionInfo.Context, actionInfo.ToInfo);

            var subjectExpr = engine.GetExpressionForValue(actionInfo.Context, actionInfo.SubjectInfo);

            var bodyExpr = engine.GetExpressionForValue(actionInfo.Context, actionInfo.BodyInfo);
            
            var smtpSection = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;

            var client = Expression.New(typeof(SmtpClient).GetConstructor(new []{typeof(string), typeof(int)}),
                new[]{Expression.Constant(smtpSection.Network.Host), Expression.Constant(smtpSection.Network.Port)});

            var message = Expression.New(typeof(MailMessage).GetConstructor(new[] { typeof(string), typeof(string), typeof(string), typeof(string)}),
                Expression.Convert(fromExpr, typeof(string)), Expression.Convert(toExpr, typeof(string)), Expression.Convert(subjectExpr, typeof(string)),
                Expression.Convert(bodyExpr, typeof(string)));

            var varExpr = Expression.Variable(typeof(MailMessage), "message");

            var callExpr = Expression.Call(client, typeof (SmtpClient).GetMethod("Send", new[] {typeof (MailMessage)}), varExpr);
            
            var expressions = new Expression[]
                {
                    Expression.Assign(varExpr, Expression.Convert(message, typeof (MailMessage))),
                    Expression.Assign(Expression.Property(varExpr, "IsBodyHtml"), Expression.Constant(true)),
                    callExpr
                };

            var sendMailExpr = Expression.Block(
                    new ParameterExpression[] { varExpr },
                    expressions
                );

            // TODO: Wrap into try/catch

            block.Code = sendMailExpr;
        }
    }
}
