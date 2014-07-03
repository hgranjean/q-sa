using NUnit.Framework;
using Rules.Domain;
using Rules.Domain.EndPoints;
using System.Net;
using Rules.Engine.Tests.Utilities;

namespace Rules.Engine.Tests
{
    [TestFixture]
    public class SendMailTest
    {
        class Entity1
        {
            public string From { get; set; }
        }

        private static readonly IPEndPoint _smtpServerMockEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 25);

        [Test]
        public void TestSendMail()
        {
            var ra = new RuleApplicationSpec();

            var ep = new SendMailSpec {Name = "server1", ServerName = _smtpServerMockEndPoint.ToString()};
            ra.EndPoints.Add(ep);

            var action1 = new SendMailAction();
            action1.From = "a";
            action1.To = "b";
            action1.Subject = "c";
            action1.Body = "d";
            action1.Server = ep.Name;

            var rs1 = new RuleSpec();
            rs1.Actions.Add(action1);
            ra.RuleSets.Add(rs1);

            using (var rs = new RuleSession(ra))
            {
                using (var smtp = new SmtpServerSession(_smtpServerMockEndPoint))
                {
                    var result = rs.ExecuteRules();
                    Assert.IsNotNull(result);
                }
            }
        }
    }
}
