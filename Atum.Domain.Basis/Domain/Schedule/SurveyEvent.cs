namespace Atum.Domain.Basis.Domain.Schedule
{
    public partial class SurveyEvent
    {
        public SurveyEvent()
        {
        }

        public string SurveyId { get; set; }
        public string EventId { get; set; }

        public virtual SurveyEntry Survey { get; set; }
        public virtual Event Event { get; set; }
    }
}
