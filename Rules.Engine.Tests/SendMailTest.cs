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
            public string To { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
        }

        private static readonly IPEndPoint _smtpServerMockEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 25);

        [Test]
        public void TestSendMail()
        {
            var ra = new RuleApplicationSpec();

            var e1 = new EntitySpec("e1", typeof (Entity1));
            ra.Entities.Add(e1);

            var ep = new SendMailSpec {Name = "server1", ServerName = _smtpServerMockEndPoint.ToString()};
            ra.EndPoints.Add(ep);

            var action1 = new SendMailAction();
            action1.From = "Context.From";
            action1.To = "Context.To";
            action1.Subject = "Context.Subject";
            action1.Body = "Context.Body";
            action1.Server = ep.Name;

            var rs1 = new RuleSpec();
            rs1.Actions.Add(action1);
            e1.RuleSets.Add(rs1);

            using (var rs = new RuleSession(ra))
            {
                rs.CreateEntity(e1.Name, new Entity1 { From = "alex@alexschmidt.net", To = "me@alexschmidt.net", Subject = "Test", Body = "Test" });
                using (var smtp = new SmtpServerSession(_smtpServerMockEndPoint))
                {
                    var result = rs.ExecuteRules();
                    Assert.IsNotNull(result);
                }
            }
        }
        
        [Test]
        public void TestSendMailReal()
        {
            var ra = new RuleApplicationSpec();

            var e1 = new EntitySpec("e1", typeof(Entity1));
            ra.Entities.Add(e1);

            var ep = new SendMailSpec {Name = "server1", ServerName = _smtpServerMockEndPoint.ToString()};
            ra.EndPoints.Add(ep);

            var action1 = new SendMailAction();
            action1.From = "Context.From";
            action1.To = "Context.To";
            action1.Subject = "Context.Subject";
            action1.Body = "Context.Body";
            action1.Server = ep.Name;

            var rs1 = new RuleSpec();
            rs1.Actions.Add(action1);
            e1.RuleSets.Add(rs1);

            using (var rs = new RuleSession(ra))
            {
                rs.CreateEntity(e1.Name, new Entity1 { From = "me@alexschmidt.net", To = "me@alexschmidt.net", Subject = "Test", Body = "Test" });
                
                var result = rs.ExecuteRules();
                Assert.IsNotNull(result);
            }
        }
    }
}
