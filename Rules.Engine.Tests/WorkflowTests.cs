using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;

namespace Rules.Engine.Tests
{
    [TestFixture]
    public class WorkflowTests
    {
        [Test]
        public void TestEntityCreatedTrigger()
        {
            // When an entity is created
            // Trigger a ruleset

            var list = new List<CollectionAggregateTests.Entity1>();
            list.Add(new CollectionAggregateTests.Entity1 { Field1 = "1" });
            list.Add(new CollectionAggregateTests.Entity1 { Field1 = "2" });
            list.Add(new CollectionAggregateTests.Entity1 { Field1 = "3" });

            var min = list.Min(t => t.Field1);

            var ra = new RuleApplicationSpec();
            var e2 = new EntitySpec("Entity2", typeof(CollectionAggregateTests.Entity2));
            ra.Entities.Add(e2);

            var action1 = new SetValueAction();
            action1.Target = "Context.TextField";
            action1.Value = "Queryable.Max(Context.EntityField, it => it.Field1)";

            var rs1 = new RuleSpec();
            rs1.Actions.Add(action1);
            e2.RuleSets.Add(rs1);

            // create an event (autoapply rules?)
            
            using (var rs = new RuleSession(ra))
            {   
                var e2val = new CollectionAggregateTests.Entity2();
                e2val.EntityField = list.AsQueryable();
                var e2Instance = rs.CreateEntity(e2.Name, e2val);

                // An entity was triggered by auto-ruleset
                var result = rs.ExecuteRules();

                Assert.IsNotNull(result);
                Assert.AreEqual("3", e2val.TextField);
            }
        }
    }
}
