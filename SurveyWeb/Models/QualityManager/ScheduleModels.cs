using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Atum.Database.Surveillance.Models;
using System;
using Atum.Domain;
using Atum.Domain.Security.Domain;
using Atum.Domain.SurveyManagement;
using Atum.Domain.Common;

namespace SurveyWeb.Models
{
    /// <summary>
    /// TODO: Define Models per View
    /// </summary>
    public class TaskViewModel : ViewModelBase
    {
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Start { get; set; }
        
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm z}")]
        public DateTime End { get; set; }

        public string OwnerName 
        {
            get { return Owner.FullName; }
        }

        public Person Owner { get; set; }

        public string CurrentState { get; set; }        
        
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
        

        public TaskViewModel()
        {
        }

        public TaskViewModel(Event model)
        {
            Id = model.Id;
            Title = model.Title;
            Start = model.Start;
            End = model.End;
        }

        public string Area { get; set; }

        public string IssuesCount { get; set; }
    }
}