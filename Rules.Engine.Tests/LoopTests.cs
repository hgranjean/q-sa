using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rules.Domain;

namespace Rules.Engine.Tests
{
    [TestFixture]
    public class LoopTests
    {
        public class Entity1
        {
            public string Field1 { get; set; }

            public Func<Entity1, string> FieldAccessor = t => t.Field1;
        }

        public class Entity2
        {
            public IQueryable<Entity1> EntityField { get; set; }
            public int ResultField { get; set; }
            public string TextField { get; set; }

            public Entity2()
            {
                this.EntityField = new List<Entity1>().AsQueryable();
            }
        }

        [Test]
        [Ignore]
        public void TestForEach()
        {
            var ra = new RuleApplicationSpec();
            var e2 = new EntitySpec("Entity2", typeof(Entity2));
            ra.Entities.Add(e2);

            var action1 = new SetValueAction();
            action1.Target = "Context.TextField";
            action1.Value = "Context.EntityField.ToList().ForEach(t => Context.ResultField++)";

            var rs1 = new RuleSpecification();
            rs1.Actions.Add(action1);
            e2.RuleSets.Add(rs1);

            using (var rs = new RuleSession(ra))
            {
                var e2val = new Entity2();
                // e2val.EntityField = list.AsQueryable();
                var e2Instance = rs.CreateEntity(e2.Name, e2val);

                var result = rs.ExecuteRules();
                Assert.IsNotNull(result);
                Assert.AreEqual("1", e2val.TextField);
            }
        }

        [Test]
        public void TestWhile()
        {
            var ra = new RuleApplicationSpec();
            var e2 = new EntitySpec("Entity2", typeof(Entity2));
            ra.Entities.Add(e2);

            var decl1 = new DeclareVariableAction();
            decl1.Name = "index";
            decl1.Value = "0";
            decl1.ValueType = typeof (int).Name;
            
            var action1 = new SetValueAction();
            action1.Target = "Context.ResultField";
            action1.Value = "Context.ResultField + index * 2";
            
            var action2 = new SetValueAction();
            action2.Target = "index";
            action2.Value = "index + 1";

            var while1 = new WhileRuleSet();
            while1.Condition = "index < 10";
            while1.Rules.AddRange(new []{action1, action2});

            var rs1 = new RuleSpecification();
            rs1.Actions.Add(decl1);
            rs1.Actions.Add(while1);
            e2.RuleSets.Add(rs1);

            using (var rs = new RuleSession(ra))
            {
                var e2val = new Entity2();
                // e2val.EntityField = list.AsQueryable();
                var e2Instance = rs.CreateEntity(e2.Name, e2val);

                var result = rs.ExecuteRules();
                Assert.IsNotNull(result);
                Assert.AreEqual("90", e2val.ResultField);
            }
        }
    }
}
