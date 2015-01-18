using Atum.Domain.Business;
using Atum.Domain.Security.Domain;

namespace Atum.Database.Surveillance.Models
{
    public partial class UserHospital
    {
        public UserHospital()
        {
        }

        public string UserId { get; set; }
        public string HospitalId { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual Hospital Hospital { get; set; }
    }
}
