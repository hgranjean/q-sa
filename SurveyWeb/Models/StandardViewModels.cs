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
    public class TOCElementViewModel
    {
        public string Title { get; set; }
        public int Level { get; set; }
        public string Content { get; set; }
        public string ShortContent { get; set; }


        public string Key { get; set; }
    }
}