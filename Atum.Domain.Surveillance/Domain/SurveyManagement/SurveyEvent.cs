using Atum.Domain.Basis.Domain.Schedule;
using Atum.Domain.Security.Domain;

namespace Atum.Domain.SurveyManagement
{
    public partial class SurveyEvent
    {
        public SurveyEvent()
        {
        }
        
        public string EventId { get; set; }
        public string UserId { get; set; }
        
        public virtual Event Event { get; set; }
        public virtual AspNetUser User { get; set; }
    }
}
