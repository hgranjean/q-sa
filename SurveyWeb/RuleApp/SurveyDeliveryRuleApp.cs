using System.Collections.Generic;
using System.Linq;
using Atum.Domain.Surveillance;
using Rules.Domain;
using Rules.Engine;

namespace SurveyWeb.RuleApp
{
    public class SurveyDeliveryRuleApp
    {
        private static RuleApplicationSpec _ruleApplication = null;

        public class EvaluationResult
        {
            public string TextResult { get; set; }
            public int Result { get; set; }
            public bool IsFollowup { get; set; }
        }


        public class SurveyEvaluator
        {
            public Questions Questions { get; set; }
            public Responses Responses { get; set; }
            public List<EvaluationResult> EvaluationResults { get; set; }
            public IQueryable<EvaluationResult> EvaluationResultsQueryable { get { return EvaluationResults.AsQueryable(); } }
            public int ResultField { get; set; }

            public SurveyEvaluator()
            {
                EvaluationResults = new List<EvaluationResult>();
            }
        }

        #region Initialize rule app
        public void InitializeRuleApp()
        {
            var ra = new RuleApplicationSpec();
            var e1 = new EntitySpec("SurveyEvaluator", typeof(SurveyEvaluator));
            ra.Entities.Add(e1);
            
            var decl1 = new DeclareVariableAction();
            decl1.Name = "index";
            decl1.Value = "0";
            decl1.ValueType = typeof(int).Name;

            var action1 = new AddCollectionMemberAction();
            action1.Target = "Context.EvaluationResults";
            
            var action2 = new SetValueAction();
            action2.Target = "Context.EvaluationResults[index].TextResult";
            action2.Value = "Context.Questions[index].GetResponseByText(Context.Responses[index].Answer.Text).Text";

            var action2_2 = new SetValueAction();
            action2_2.Target = "Context.EvaluationResults[index].Result";
            action2_2.Value = "Context.EvaluationResults[index].Result + 1";

            var action2_3 = new SetValueAction();
            action2_3.Target = "Context.EvaluationResults[index].IsFollowup";
            action2_3.Value = @"Context.Responses[index].Answer.Text == ""Follow-Up Completed""";

            var action3 = new SimpleRuleSet();
            action3.Condition = "Context.Responses.Count > index"; // Perform some analysis here
            action3.Rules.Add(action2);
            action3.Rules.Add(action2_2);
            action3.Rules.Add(action2_3);

            var action4 = new SetValueAction();
            action4.Target = "index";
            action4.Value = "index + 1";

            var action5 = new SetValueAction();
            action5.Target = "Context.ResultField";
            action5.Value = "Context.EvaluationResultsQueryable.Sum(t => t.Result)"; // Set to value depending on the evaluation, currently using analytical func here

            var while1 = new WhileRuleSet();
            while1.Condition = "index < Context.Questions.Count";
            while1.Rules.Add(action1);
            while1.Rules.Add(action3);
            while1.Rules.Add(action4);
            while1.Rules.Add(action5);

            var rs1 = new RuleSpec();
            rs1.Actions.Add(decl1);
            rs1.Actions.Add(while1);
            e1.RuleSets.Add(rs1);

            _ruleApplication = ra;
        }
        #endregion

        private static SurveyEvaluator CreateSurveyEvaluator(Questions questions, Responses responses)
        {
            var e2val = new SurveyEvaluator();
            
            // Setup questionnaire
            e2val.Questions = new Questions();
            // Setup questionnaire
            e2val.Questions = new Questions();
            e2val.Questions.AddRange(questions);

            // Setup responses from user
            e2val.Responses = new Responses();
            e2val.Responses.AddRange(responses);

            /*e2val.Questions.Add(new Question());
            e2val.Questions.Add(new Question());
            e2val.Questions.Add(new Question());
            e2val.Questions[0].AddChoice("0");
            e2val.Questions[1].AddChoice("1");
            e2val.Questions[2].AddChoice("2");
            e2val.Responses.Add(new Response(e2val.Questions[0], new ResponseChoice("0")));
            e2val.Responses.Add(new Response(e2val.Questions[1], new ResponseChoice("1")));
            e2val.Responses.Add(new Response(e2val.Questions[2], new ResponseChoice("2")));*/

            return e2val;
        }

        public List<EvaluationResult> EvaluateSurvey(Questions questions, Responses responses)
        {
            var e1 = _ruleApplication.Entities.First();

            using (var rs = new RuleSession(_ruleApplication))
            {
                var e1val = CreateSurveyEvaluator(questions, responses);
                var e1Instance = rs.CreateEntity(e1.Name, e1val);

                var result = rs.ExecuteRules();

                return e1val.EvaluationResults;
            }
        }
    }
}