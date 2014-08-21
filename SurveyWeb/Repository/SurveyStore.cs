using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Repository
{
    public enum StoreType
    {
        JointCommissionStandards,
        OpenNLP,
        Emails,
        Responses,
        Surveys,
        Standards
    }

    public interface ISurveyStore
    {
        string Location { get; }

        string GetPath(StoreType storeType);
    }

    public class SurveyStore : ISurveyStore
    {
        private readonly string _location;

        public SurveyStore(string location)
        {
            _location = location;
        }

        public string Location { get { return _location; }}


        public string GetPath(StoreType storeType)
        {
            string appPath = HttpContext.Current.Server.MapPath("~/bin");

            int binPos = appPath.LastIndexOf(@"\bin", StringComparison.CurrentCultureIgnoreCase);

            appPath = appPath.Substring(0, binPos) + @"\Store\" + storeType.ToString();

            return appPath;
        }
    }
}