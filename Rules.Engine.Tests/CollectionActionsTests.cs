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
    public class CollectionActionsTests
    {
        [Test]
        public void TestAddCollectionMember()
        {
            var ra = new RuleApplicationSpec();
            var e3 = new EntitySpec("Entity2", typeof(RuleSessionTests.Entity3));
            ra.Entities.Add(e3);

            var action1 = new AddCollectionMemberAction();
            action1.Target = "Context.EntityField";

            var conditionalRuleSet = new SimpleRuleSet();
            conditionalRuleSet.Condition = "Context.EntityField.Count == 0";
            conditionalRuleSet.Rules.Add(action1);

            var rs1 = new RuleSpec();
            rs1.Actions.Add(conditionalRuleSet);
            e3.RuleSets.Add(rs1);

            using (var rs = new RuleSession(ra))
            {
                var e3val = new RuleSessionTests.Entity3();

                var e3Instance = rs.CreateEntity(e3.Name, e3val);

                var result = rs.ExecuteRules();
                Assert.IsNotNull(result);
                Assert.AreEqual(1, e3val.EntityField.Count);
            }
        }

        [Test]
        public void TestSetCollectionMemberField()
        {
            var ra = new RuleApplicationSpec();
            var e3 = new EntitySpec("Entity2", typeof(RuleSessionTests.Entity3));
            ra.Entities.Add(e3);

            var action1 = new AddCollectionMemberAction();
            action1.Target = "Context.EntityField";

            var action2 = new SetValueAction();
            action2.Target = "Context.EntityField[0].Field1";
            action2.Value = "1234";

            var conditionalRuleSet = new SimpleRuleSet();
            conditionalRuleSet.Condition = "Context.EntityField.Count == 0";
            conditionalRuleSet.Rules.Add(action1);
            conditionalRuleSet.Rules.Add(action2);

            var rs1 = new RuleSpec();
            rs1.Actions.Add(conditionalRuleSet);
            e3.RuleSets.Add(rs1);

            using (var rs = new RuleSession(ra))
            {
                var e3val = new RuleSessionTests.Entity3();

                var e3Instance = rs.CreateEntity(e3.Name, e3val);

                var result = rs.ExecuteRules();
                Assert.IsNotNull(result);
                Assert.AreEqual(1, e3val.EntityField.Count);
                Assert.AreEqual("1234", e3val.EntityField[0].Field1);
            }
        }

        // Implement:
        // Clear
        // Remove
    }
}
