using System;

namespace Atum.Database.Surveillance.Models
{
    public class Event
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Url { get; set; }
    }
}
