using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atum.Domain.Surveillance;
using NUnit.Framework;
using Rules.Domain;
using Rules.Engine;

namespace MvcApplication1.Tests
{
    [TestFixture]
    public class SurveyEvaluatorTest
    {
        private static RuleApplicationSpec _ruleApplication = null;

        public class EvaluationResult
        {
            public int Result { get; set; }    
        }

        public class SurveyEvaluatorRequest
        {
            public Questions Questions { get; set; }
            public Responses Responses { get; set; }
            public List<EvaluationResult> EvaluationResults { get; set; }
            public int ResultField { get; set; }

            public SurveyEvaluatorRequest()
            {
                EvaluationResults = new List<EvaluationResult>();
            }
        }

        #region Initialize rule app
        public void InitializeRuleApp()
        {
            var ra = new RuleApplicationSpec();
            var e1 = new EntitySpec("SurveyEvaluatorRequest", typeof(SurveyEvaluatorRequest));
            ra.Entities.Add(e1);

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
            e1.RuleSets.Add(rs1);

            _ruleApplication = ra;

        }
        #endregion

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            InitializeRuleApp();    
        }

        [Test]
        public void TestSurveyEvaluator()
        {
            var e1 = _ruleApplication.Entities.First();

            using (var rs = new RuleSession(_ruleApplication))
            {
                var e2val = new SurveyEvaluatorRequest();
                // e2val.EntityField = list.AsQueryable();
                var e2Instance = rs.CreateEntity(e1.Name, e2val);

                var result = rs.ExecuteRules();
                Assert.IsNotNull(result);
                Assert.AreEqual(90, e2val.ResultField);
            }
        }
    }
}
