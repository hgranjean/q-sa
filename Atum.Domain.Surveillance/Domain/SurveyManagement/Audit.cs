using Atum.Domain.Basis;
using Atum.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.SurveyManagement
{
    /// <summary>
    /// This class represents a Surveillance Performed
    /// </summary>
    public class Audit : DomainObject
   {
        public Audit()
        {
        }

        public Audit(string surveyTitle, Person surveyor, int surveyId)
        {
            this.SurveyTitle = surveyTitle;
            this.Surveyor = surveyor;
            this.SurveyId = surveyId;
        }

        public Person Surveyor { get; set; }
        public string SurveyTitle { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateCompleted { get; set; }
        public Responses Responses { get; set; }
        public int SubcriberId { get; set; }
        public int Score { get; set; }
        public int SurveyId { get; set; }
   }
}
