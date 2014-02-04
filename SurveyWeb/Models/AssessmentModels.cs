using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Atum.Domain.Surveillance;
using SurveyWeb.Services;
using Repository;
using Runtime;
using Entity = Runtime.Entity;

namespace SurveyWeb.Models
{
    public class AssessmentViewModel : ViewModelBase
    {
        //Entity member variable should *not* be static - its lifetime is for the request only
        public Assessment CurrentAssessment { get; private set; }
        public Survey.QuestionEnumerator Enumerator { get; private set; }

        
        // public RuleSessionState SessionState { get; private set; }

        
        public AssessmentViewModel()
        {
            CurrentAssessment = SessionBag.Current.CurrentAssessment ?? LoadAssessment();
            Enumerator = SessionBag.Current.Enumerator;
        }

        private Assessment LoadAssessment()
        {
            var questions = new Questions();
            questions.Add(new Question(
                        "Leaders identify an individual(s) to manage risk, coordinate risk, reduction activities in the physical environment, collect deficiency information, and disseminate summaries of actions, and results.",
                        QuestionType.Ranking){ Rank = 0});
            questions.Add(new Question(
                        "Leaders identify an individual(s) to intervene whenever environmental conditions immediately threaten life or health or threaten to damage equipment or buildings.",
                        QuestionType.Ranking) { Rank = 1});
            questions.Add(new Question(
                        "The environmental safety of patients and  everyone who enters the hospital’s facilities. (See also EC.04.01.01, EP 15).",
                        QuestionType.Ranking) { Rank = 2 });
            
            var strategy = new SurveyStrategy(questions);
            var survey = new Survey(strategy);

            var assessment = new Assessment
                {
                    ConductedSurvey = survey,
                    Responses = new SurveyResponse {Responses = new Responses(), Survey = survey}
                };

            
            var manager = new SurveyManager(assessment.ConductedSurvey);

            Enumerator = (Survey.QuestionEnumerator)assessment.ConductedSurvey.GetEnumerator();
            Enumerator.SetSurveyManager(manager);

            assessment.ConductedSurvey.FirstQuestion = manager.CurrentQuestion;
            
            SessionBag.Current.CurrentAssessment = assessment;
            SessionBag.Current.Enumerator = Enumerator;
            
            return assessment;
        }
    }

    public abstract class ViewModelBase
    {
        private static RuleApplicationDef rulesApp;
        public RuleSession Session { get; private set; }

        protected static void LoadRules()
        {
            // rulesApp = RuleApplicationDef.Load(GetRuleAppFileName());
        }

        protected string GetRuleAppFileName()
        {
            string appPath = HttpContext.Current.Server.MapPath("~/bin");
            int binPos = appPath.LastIndexOf(@"\bin");

            appPath = appPath.Substring(0, binPos) + @"\RuleApp\";
            
            return appPath + ConfigurationSettings.AppSettings.Get("RuleApp");
        }

        private string GetRuleAppStateFileName()
        {
            string appPath = HttpContext.Current.Server.MapPath("~/bin");
            int binPos = appPath.LastIndexOf(@"\bin");

            appPath = appPath.Substring(0, binPos) + @"\RuleApp\";

            return appPath + ConfigurationSettings.AppSettings.Get("RuleAppState");
        }

        /*
        /// <summary>
        /// Create <see cref="RuleSession"/> for use during this page request only, 
        ///	using persistent <see cref="RuleSessionState"/> from ASP.NET session state if any.
        /// If no ASP.NET session state yet stored, a new one is created here.
        /// </summary>
        /// <returns></returns>
        protected RuleSession CreateRuleSession()
        {
            //Get from ASP.NET Session State
            var state = this.SessionState;
            if (state == null)
            {
                state = new RuleSessionState();
            }

            var session = this.Session;
            if (session == null)
            {
                var ruleAppSource = RuleApplicationDef.Load(GetRuleAppFileName());

                //Set up appropriate connection based on config setting
                session = new RuleSession(ruleAppSource, CacheRetention.AlwaysRetain);
                session.SaveState();
                // Preserve the working memory between subsequent requests to engine service
                // session.RuleExecutionSettings.WorkingMemoryBehavior = WorkingMemoryBehavior.Default;
            }

            //Store in ASP.NET Session
            this.Session = session;

            return session;
        }*/

    }

    public class AdministeredAssessmentViewModel
    {
        public Assessment AdministeredAssessment { get; set; }    
    }
}