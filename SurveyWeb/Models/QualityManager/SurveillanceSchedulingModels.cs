using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.QualityManager.SurveillanceManagement
{
    public class SurveillanceSchedulingViewModels
    {
    }

    public class SurveillanceManagementModel
    {
        public List<SurveillanceScheduleAnnualViewModel> AreaAnnualSchedule { get; set; }
    }

    public class SurveillanceScheduleAnnualViewModel
    {
        public string Area { get; set; }
        public string January { get; set; }
        public string February { get; set; }
        public string March { get; set; }
        public string April { get; set; }
        public string May { get; set; }
        public string June { get; set; }
        public string July { get; set; }
        public string August { get; set; }
        public string September { get; set; }
        public string October { get; set; }
        public string November { get; set; }
        public string December { get; set; }
        
    }
}