using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models
{   public class ProgressReportViewModel
    {
        private readonly ProgressReport _model;
        public ProgressReportViewModel(ProgressReport model)    
        {
            _model = model;
        }
        public decimal[,] ElementsByAreas { get { return _model.ElementsByAreas; } }
    }

    public class ProgressReport
    {
        public decimal[,] ElementsByAreas { get; private set; }

        public ProgressReport(decimal[,] elementsByAreas)
        {
            this.ElementsByAreas = elementsByAreas;
        }
    }
}