using Atum.Domain.Basis.Domain.Schedule;
using Atum.Domain.Security.Domain;

namespace Atum.Domain.SurveyManagement
{
    public partial class xSurveyEvent
    {
        public xSurveyEvent()
        {
        }
        
        public string EventId { get; set; }
        public string UserId { get; set; }
        
        public virtual Event Event { get; set; }
        public virtual AspNetUser User { get; set; }
    }
}
