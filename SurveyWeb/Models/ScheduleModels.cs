using System.ComponentModel.DataAnnotations;
using Atum.Database.Surveillance.Models;
using System;
using Atum.Domain.Basis.Domain.Schedule;

namespace SurveyWeb.Models
{
    public class EventViewModel : ViewModelBase
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime Start { get; set; }
        
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm z}")]
        public DateTime End { get; set; }
        
        public string Url { get; set; }

        public EventViewModel()
        {
        }

        public EventViewModel(Event evt)
        {
            Id = evt.Id;
            Title = evt.Title;
            Start = evt.Start;
            End = evt.End;
        }
    }
}