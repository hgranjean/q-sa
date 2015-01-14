using Atum.Domain.QualityManagement.Healthcare.Performance;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SurveyWeb.Models.StandardMaintenance
{
    public class StandardViewModels : ViewModelBase
    {

    }

    public class StandardViewModel : StandardDocumentViewModel
    {
        public int StandardTypeId { get; set; }

    }


    public class StandardSearchViewModel
    {
        [Display(Name = "Document")]
        public string StandardId { get; set; }
        public IEnumerable<StandardType> StandardTypes { get; set; }
        [Display(Name = "Type")]
        public int StandardTypeId { get; set; }
        [Display(Name = "Element of Performance")]

        public List<StandardDocumentViewModel> Results { get; set; }
        //        public string Content { get; set; }
    }

    public class StandardType 
    {
        private int p1;
        private string p2;

        public StandardType(int p1, string p2)
        {
            // TODO: Complete member initialization
            this.Id = p1;
            this.Name = p2;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class StandardDocumentViewModel
    {
        public int Id { get; set; }
        public string Key { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        public string Text { get; set; }
        public IEnumerable<TOCElementViewModel> TableOfContents { get; set; }
        [Display(Name = "Chapter")]
        public TOCElementViewModel ChapterItem { get; set; }
        public Atum.Domain.Common.Person Owner { get; set; }
        public string OwnerName{get { return Owner.FullName; }}
        public string Visibility { get; set; }
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
        [Display(Name = "Standard")]
        public string StandardId { get; set; }
        public string Content { get; set; }
        [Display(Name = "Element of Performance")]
        [Required]
        public IEnumerable<string> EPIds { get; set; }
        public string Observation { get; set; }

        public StandardElementViewModel()
        {
        }

        public StandardElementViewModel(Standard model)
        {
            this.StandardId = model.Key;
            //this.Content = model.Content;
            this.EPIds = getStrings(model.PerformanceItems);
            //this.Observation = model.Observation;
        }

        private IEnumerable<string> getStrings(Atum.Domain.Common.DocumentElements documentElements)
        {
            throw new System.NotImplementedException();
        }

        private IEnumerable<string> getStrings(IEnumerable<PerformanceItem> enumerable)
        {
            throw new System.NotImplementedException();
        }
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