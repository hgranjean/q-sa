using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Models
{
    public class StandardViewModels : ViewModelBase
    {

    }


    public class StandardDocumentViewModel
    {
        public string Title { get; set; }
        public List<TOCElementViewModel> TableOfContents { get; set; }
    
    }

    public class PerformanceElementViewModel
    {
        public string EPId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Notes { get; set; }
        public List<HtmlString> ReferencedElementLinks { get; set; }

        public string StandardId { get; set; }
    }
    
    public class StandardElementViewModel 
    {
        public string StandardId { get; set; }
        public IEnumerable<string> EPIds { get; set; }


        public string Observation { get; set; }
    }
    public class TOCElementViewModel
    {
        public string Title { get; set; }
        public int Level { get; set; }
        public string Content { get; set; }
        public string ShortContent { get; set; }
        public string ParentKey {get;set; }

        public string Key { get; set; }

        public List<TOCElementViewModel> Elements { get; set; }
    }
}