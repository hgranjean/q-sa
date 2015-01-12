using Atum.Domain.Security.Domain;
using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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



    /// <summary>
    /// TODO: Define Models per View
    /// </summary>
    public class SurveilanceViewModel : ViewModelBase
    {
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm z}")]
        public DateTime End { get; set; }

        public SurveyEntry Survey { get; set; }
        public IEnumerable<SurveyEntry> AvailableSurveys { get; set; }

        [Display(Name = "Selected Users")]
        public IEnumerable<AspNetUser> Users { get; set; }

        [Display(Name = "Available Users")]
        public IEnumerable<AspNetUser> AvailableUsers { get; set; }

        [Display(Name = "Selected Users")]
        public IEnumerable<string> SelectedUsers { get; set; }

        [Display(Name = "Template")]
        public string SurveyId { get; set; }

        public SurveilanceViewModel()
        {
        }

        public SurveilanceViewModel(Event model)
        {
            Id = model.Id;
            Title = model.Title;
            Start = model.Start;
            End = model.End;
        }
    }

}