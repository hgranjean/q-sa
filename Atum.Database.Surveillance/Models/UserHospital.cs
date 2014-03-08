using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
