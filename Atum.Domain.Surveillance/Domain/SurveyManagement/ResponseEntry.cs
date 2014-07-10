using Atum.Domain.Security.Domain;

namespace Atum.Domain.SurveyManagement
{
    public partial class ResponseEntry
    {
        public string Id { get; set; }

        public virtual AspNetUser User { get; set; }
        public string UserId { get; set; }
    }
}
