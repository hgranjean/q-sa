using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models
{
    public class NLPModels
    {
    }

    public class TrainingDocumentViewModel
    {
        public List<string> WordList { get; set; }
        public string Text { get; set; }
        public string StandardText { get; set; }
        public string Class { get; set; }
        public string DocumentId { get; set; }
        public IEnumerable<string> ClassList { get; set; }


        public string StandardTitle { get; set; }
    }

    public class NLPNBStatisticsViewModel
    {
        
    }
}