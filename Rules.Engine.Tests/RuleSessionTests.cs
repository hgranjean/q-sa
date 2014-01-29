using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rules.Domain;

namespace Rules.Engine.Tests
{
    [TestFixture]
    public class RuleSessionTests
    {
        [Test]
        public void TestSetValueAction()
        {
            var ra = new RuleApplicationSpec();

            var action1 = new SetValueAction();
            action1.Target = "a";
            action1.Value = "1234";

            var rs1 = new RuleSpecification();
            rs1.Actions.Add(action1);
            ra.RuleSets.Add(rs1);
            
            using (var rs = new RuleSession(ra))
            {
                var result = rs.ExecuteRules();
                Assert.IsNotNull(result);
            }
        }

        public class Entity1
        {
            public String Field1 { get; set; }
            public int Field2 { get; set; }

            public void SetField1(String value1)
            {
                this.Field1 = value1;
            }

            public void SetField2(int value2)
            {
                this.Field2 = value2;
            }
        }

        [Test]
        public void TestSetFieldOfEntity()
        {
            var ra = new RuleApplicationSpec();
            var e1 = new EntitySpec("Entity1", typeof (Entity1));
            ra.Entities.Add(e1);

            var action1 = new SetValueAction();
            action1.Target = "Field1";
            action1.Value = "1234";

            var rs1 = new RuleSpecification();
            rs1.Actions.Add(action1);
            e1.RuleSets.Add(rs1);

            using (var rs = new RuleSession(ra))
            {
                var e1Instance = rs.CreateEntity(e1.Name, new Entity1());

                var result = rs.ExecuteRules();
                Assert.IsNotNull(result);
            }
        }

        [Test]
        public void TestExecuteRuleSet()
        {
            var ra = new RuleApplicationSpec();
            var e1 = new EntitySpec("Entity1", typeof(Entity1));
            ra.Entities.Add(e1);

            var action1 = new SetValueAction();
            action1.Target = "Field1";
            action1.Value = "1234";

            var rs1 = new RuleSpecification();
            rs1.Actions.Add(action1);
            e1.RuleSets.Add(rs1);

            using (var rs = new RuleSession(ra))
            {
                var e1Instance = rs.CreateEntity(e1.Name, new Entity1());

                var result = rs.ExecuteRules();
                Assert.IsNotNull(result);
            }
        }

        [Test]
        public void TestPureExpr()
        {
            Expression<Func<int, bool>> expr = num => num < 5;

            Object result = (bool) expr.Compile()(3);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void TestFilter()
        {
            // http://stackoverflow.com/questions/18663345/building-expression-tree-using-a-parameters-indexer
        }
    }
}
