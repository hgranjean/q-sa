using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using NUnit.Framework;
using Rules.Domain;
using Rules.Engine.Session;

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

        class MyClass : DynamicObject
        {
            private object value;
            public MyClass(object value)
            {
                this.value = value;
            }
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                result = this.value;
                return true;
                // return base.TryGetMember(binder, out result);
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                this.value = value;
                return true;
                // return base.TrySetMember(binder, value);
            }
        }

        [Test]
        public void TestDynamic()
        {
            ;
        }

        [Test]
        public void TestExecuteRuleSet()
        {
            dynamic t = "1";
            // var t = "Hello";
            // var s = new MyClass(t);
            // var s2 = s.Field1;

            var ra = new RuleApplicationSpec();
            var e1 = new EntitySpec("Entity1", typeof(Entity1));
            ra.Entities.Add(e1);

            var action1 = new SetValueAction();
            action1.Target = "Context.Field2";
            action1.Value = "1234";

            var conditionalRuleSet = new SimpleRuleSet();
            conditionalRuleSet.Condition = "Context.Field1 == \"1234\"";
            conditionalRuleSet.Rules.Add(action1);

            var rs1 = new RuleSpecification();
            rs1.Actions.Add(conditionalRuleSet);
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
