using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.QualityAuditor.SurveillancePerformance
{
    public class SurveillanceViewModel
    {
    }

    public class SurveillancesViewModel 
    {
        public IEnumerable<TaskViewModel> Surveillances { get; set; }
    }

    public class ObservationsViewModel
    {
        public IEnumerable<ObservationViewModel> Observations { get; set; }
        
    }

}