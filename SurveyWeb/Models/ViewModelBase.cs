using System.Configuration;
using System.Web;
using System.Web.Script.Serialization;
using Repository;
using Runtime;

namespace SurveyWeb.Models
{
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

            return appPath + ConfigurationManager.AppSettings.Get("RuleApp");
        }

        private string GetRuleAppStateFileName()
        {
            string appPath = HttpContext.Current.Server.MapPath("~/bin");
            int binPos = appPath.LastIndexOf(@"\bin");

            appPath = appPath.Substring(0, binPos) + @"\RuleApp\";

            return appPath + ConfigurationManager.AppSettings.Get("RuleAppState");
        }

        /*
        /// <summary>C:\Atum Technology Group\Projects\Mihalik\Solutions\TMGScreener
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

        public static string Serialize<T>(T xyz)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(xyz);
        }

        // This will convert the passed JSON string back to XYZ object
        public static T Deserialize<T>(string data)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(data);
        }

    }
}