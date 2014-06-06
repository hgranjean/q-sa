using Atum.Domain.Common;
using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.QualityManagement
{
    public class Surveillance
    {
        public Surveillance()
        {

        }

        public Surveillance(Survey survey)
        {
            this.Survey = survey;
        }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Frequency Frequency { get; set; }

        public string Title { get; set; }

        public Survey Survey { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
