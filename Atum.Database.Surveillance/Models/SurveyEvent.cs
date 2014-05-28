using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Database.Surveillance.Models
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
