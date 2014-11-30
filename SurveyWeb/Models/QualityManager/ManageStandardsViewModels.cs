using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models.QualityManager
{
    public class ManageStandardsViewModels
    {
    }

    public class ManageStandardsViewModel
    {
        public IEnumerable<StandardMaintenance.StandardDocumentViewModel> Guidelines { get; set; }
    }
}