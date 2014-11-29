using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.QualityManager.SurveillanceTracking
{
    public class SurveillanceTrackingViewModels
    {
    }

    public class SurveillanceTrackingViewModel
    {
        public List<SurveillanceCompletionViewModel> AreaSurveillanceCompletion { get; set; }
    }

        public class SurveillanceCompletionViewModel
        {
            public string Area { get; set; }
            public string PercentComplete { get; set; }
            // Need to diplay each as a column
            public List<FieldData> Data { get; set; }
        }
}