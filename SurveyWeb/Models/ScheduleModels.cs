using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Atum.Database.Surveillance.Models;
using System;
using Atum.Domain.Basis.Domain.Schedule;
using Atum.Domain.Security.Domain;

namespace SurveyWeb.Models
{
    public class EventViewModel : ViewModelBase
    {
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Start { get; set; }
        
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm z}")]
        public DateTime End { get; set; }
        
        public string Url { get; set; }

        public IEnumerable<AspNetUser> AvailableUsers { get; set; }

        [Display(Name = "Owner")]
        public string UserId { get; set; }

        public EventViewModel()
        {
        }

        public EventViewModel(Event model)
        {
            Id = model.Id;
            Title = model.Title;
            Start = model.Start;
            End = model.End;
            UserId = model.UserId;
        }
    }
}