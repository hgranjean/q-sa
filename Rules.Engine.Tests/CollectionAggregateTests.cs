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
    public class CollectionAggregateTests
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
        public void TestMin()
        {
            // Semantically equivalent to:
            
            var list = new List<Entity1>();
            list.Add(new Entity1 { Field1 = "1"});
            list.Add(new Entity1 { Field1 = "2"});
            list.Add(new Entity1 { Field1 = "3"});

            var min = list.Min(t => t.Field1);

            var ra = new RuleApplicationSpec();
            var e2 = new EntitySpec("Entity2", typeof(Entity2));
            ra.Entities.Add(e2);

            var action1 = new SetValueAction();
            action1.Target = "Context.TextField";
            action1.Value = "Context.EntityField.Min(t => t.Field1)";
            
            var rs1 = new RuleSpec();
            rs1.Actions.Add(action1);
            e2.RuleSets.Add(rs1);

            using (var rs = new RuleSession(ra))
            {
                var e2val = new Entity2();
                e2val.EntityField = list.AsQueryable();
                var e2Instance = rs.CreateEntity(e2.Name, e2val);

                var result = rs.ExecuteRules();
                Assert.IsNotNull(result);
                Assert.AreEqual("1", e2val.TextField);
            }
        }
        
        [Test]
        public void TestOrderBy()
        {
            // Semantically equivalent to:
            
            var list = new List<Entity1>();
            list.Add(new Entity1 { Field1 = "3" });
            list.Add(new Entity1 { Field1 = "1" });
            list.Add(new Entity1 { Field1 = "2" });

            Func<Entity1, string> func = t => t.Field1;

            var first = list.AsQueryable().OrderBy(func).First();

            var ra = new RuleApplicationSpec();
            var e2 = new EntitySpec("Entity2", typeof(Entity2));
            ra.Entities.Add(e2);

            var action1 = new SetValueAction();
            action1.Target = "Context.ResultField";
            action1.Value = "Context.EntityField.OrderBy(t => t.Field1).Count()";
            
            var rs1 = new RuleSpec();
            rs1.Actions.Add(action1);
            e2.RuleSets.Add(rs1);

            using (var rs = new RuleSession(ra))
            {
                var e2val = new Entity2();
                e2val.EntityField = list.AsQueryable();
                var e2Instance = rs.CreateEntity(e2.Name, e2val);

                var result = rs.ExecuteRules();
                Assert.IsNotNull(result);
                Assert.AreEqual(3, e2val.ResultField);
            }
        }

        
    }
}
