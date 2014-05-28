using Atum.Database.Surveillance.Models;
using System;

namespace SurveyWeb.Models
{
    public class EventViewModel : ViewModelBase
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Url { get; set; }

        public EventViewModel(Event evt)
        {
            Id = evt.Id;
            Title = evt.Title;
            Start = evt.Start;
            End = evt.End;
        }
    }
}