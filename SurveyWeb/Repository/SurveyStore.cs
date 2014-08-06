using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Repository
{
    public interface ISurveyStore
    {
        string Location { get; }

        string GetPath();
    }

    public class SurveyStore : ISurveyStore
    {
        private readonly string _location;

        public SurveyStore(string location)
        {
            _location = location;
        }

        public string Location { get { return _location; }}


        public string GetPath()
        {
            string appPath = HttpContext.Current.Server.MapPath("~/bin");

            int binPos = appPath.LastIndexOf(@"\bin", StringComparison.CurrentCultureIgnoreCase);

            appPath = appPath.Substring(0, binPos) + @"\Store\";

            return appPath;
        }
    }
}