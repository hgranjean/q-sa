using System;using NUnit.Framework;
using Rules.Domain;
using Rules.Domain.Vocabulary;

namespace Rules.Engine.Tests
{
    [TestFixture]
    public class TemplateTests
    {
        [Test]
        public void AddTemplate()
        {
            var ra = new RuleApplicationSpec();
            var e1 = new EntitySpec("Entity1", typeof(Entity1));
            ra.Entities.Add(e1);

            var templ1 = new TemplateSpec();
            templ1.DisplayText = "Test1";
            templ1.FunctionName = "Test1";
            templ1.Expression = "Context.Field1 == \"1234\"";
            templ1.Prototype = "My super rule is valid";
            ra.Vocabulary.Templates.Add(templ1);
            
            var action1 = new SetValueAction();
            action1.Target = "Context.Field2";
            action1.Value = "1234";

            var rule1 = new SimpleRuleSet();
            rule1.Condition = "Test1()";
            rule1.Rules.Add(action1);

            var rs1 = new RuleSpec();
            rs1.Actions.Add(rule1);
            e1.RuleSets.Add(rs1);

            using (var rs = new RuleSession(ra))
            {
                var e1val = new Entity1 { Field1 = "1234" };
                var e1Instance = rs.CreateEntity(e1.Name, e1val);

                var result = rs.ExecuteRules();
                Assert.IsNotNull(result);
                Assert.AreEqual("1234", e1val.Field2);
            }
        }

        public class Entity1
        {
            public String Field1 { get; set; }
            public String Field2 { get; set; }

            public void SetField1(String value1)
            {
                this.Field1 = value1;
            }

            public void SetField2(String value2)
            {
                this.Field2 = value2;
            }
        }
    }
}
