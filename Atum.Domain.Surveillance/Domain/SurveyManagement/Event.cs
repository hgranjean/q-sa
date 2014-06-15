using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atum.Domain.Basis;
using Atum.Domain.Security.Domain;

namespace Atum.Domain.SurveyManagement
{
    public partial class Event
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string SurveyId { get; set; }
        public SurveyEntry Survey { get; set; }

        // public string UserId { get; set; }
        // public AspNetUser User { get; set; }
    }
}
