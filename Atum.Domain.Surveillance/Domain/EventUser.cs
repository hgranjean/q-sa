using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atum.Domain.Security.Domain;
//using Atum.Domain.QualityManagement;

namespace Atum.Domain
{
    public partial class xEventUser
    {
        public xEventUser()
        {
        }

        public string EventId { get; set; }
        public string UserId { get; set; }

        //public virtual Surveillance Event { get; set; }
        public virtual AspNetUser User { get; set; }
    }
}
