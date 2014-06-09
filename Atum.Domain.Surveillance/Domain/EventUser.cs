using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atum.Domain.Security.Domain;
using Atum.Domain.SurveyManagement;

namespace Atum.Domain
{
    public partial class EventUser
    {
        public EventUser()
        {
        }

        public string EventId { get; set; }
        public string UserId { get; set; }

        public virtual Event Event { get; set; }
        public virtual AspNetUser User { get; set; }
    }
}
