using System;

namespace Atum.Domain.Basis.Domain.Schedule
{
    public partial class Event
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Url { get; set; }
        public string UserId { get; set; }
    }
}
