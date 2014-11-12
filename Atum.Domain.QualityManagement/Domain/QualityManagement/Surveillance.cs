using Atum.Domain.Basis;
using Atum.Domain.Common;
using Atum.Domain.SurveyManagement;
using System;

namespace Atum.Domain.QualityManagement
{

    /// <summary>
    /// 
    /// </summary>
    public class Surveillance : DomainObject
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
