using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models
{   public class ProgressReportViewModel
    {
        private decimal[,] elementsByAreas;

        public ProgressReportViewModel(decimal[,] elementsByAreas)
        {
            this.elementsByAreas = elementsByAreas;
        }
    }
}