using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Repository;
using Runtime;
using Entity = Runtime.Entity;

namespace MvcApplication1.Models
{
    public class AssessmentViewModel : ViewModelBase
    {
        //Entity member variable should *not* be static - its lifetime is for the request only
        public Entity Assessment { get; private set; }
        // public RuleSessionState SessionState { get; private set; }

        static AssessmentViewModel()
        {
            LoadRules();
        }

        public AssessmentViewModel()
        {
            Assessment = LoadAssessment();
        }

        private Entity LoadAssessment()
        {
            return null;
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
}