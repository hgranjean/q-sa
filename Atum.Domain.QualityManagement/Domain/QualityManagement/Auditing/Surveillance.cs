using Atum.Domain.Basis;
using Atum.Domain.Common;
using Atum.Domain.SurveyManagement;
using System;

namespace Atum.Domain.QualityManagement.Auditing
{

    /// <summary>
    /// 
    /// </summary>
    public class Surveillance : Event//DomainObject
    {   
        public Surveillance()
        {
        }

        public Surveillance(Survey survey)
        {
            this.Template = survey;
        }

        public Area Area { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        public Frequency Frequency { get; set; }
        public Survey Template { get; set; }
        public DateTime CreatedDate { get; set; }
        public Person CreatedBy
        {
            get;
            set;
        }
        public Person AssignedTo
        {
            get;
            set;
        }


    }
}
