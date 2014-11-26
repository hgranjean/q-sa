using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace SurveyWeb.Models.QualityManager
{
    public class ManagerUserModel
    {
        public RouteValueDictionary FollowUpCriteria { get; set; }
        public IDictionary<string, object> SurveillanceScheduleCriteria { get; set; }
        public IDictionary<string, object> TrendReportsCriteria { get; set; }
        public IDictionary<string, object> PIProgramsCriteria { get; set; }
        public IDictionary<string, object> StandardsCriteria { get; set; }
        public IDictionary<string, object> TemplatesCriteria { get; set; }
        public IDictionary<string, object> ResourcesCriteria { get; set; }
        public IDictionary<string, object> AlertsCriteria { get; set; }
         
    } 
}