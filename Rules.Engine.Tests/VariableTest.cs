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
    public class VariableTest
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
        public void TestDeclareVar()
        {
            var ra = new RuleApplicationSpec();
            var e1 = new EntitySpec("Entity1", typeof(Entity1));
            ra.Entities.Add(e1);

            var action1 = new DeclareVariableAction();
            action1.Name = "var1";
            action1.Value = "1234";
            action1.ValueType = typeof (String).Name;

            var action2 = new SetValueAction();
            action2.Target = "Context.Field1";
            action2.Value = "var1";

            var rs1 = new RuleSpec();
            rs1.Actions.Add(action1);
            rs1.Actions.Add(action2);
            e1.RuleSets.Add(rs1);

            using (var rs = new RuleSession(ra))
            {
                var e1val = new Entity1();
                var e1Instance = rs.CreateEntity(e1.Name, e1val);

                var result = rs.ExecuteRules();
                Assert.IsNotNull(result);
                Assert.AreEqual("1234", e1val.Field1);

                var e2val = new Entity1();
                var e2Instance = rs.CreateEntity(e1.Name, e2val);
                result = rs.ExecuteRules();
                Assert.AreEqual("1234", e2val.Field1);
            }
        }
    }
}
