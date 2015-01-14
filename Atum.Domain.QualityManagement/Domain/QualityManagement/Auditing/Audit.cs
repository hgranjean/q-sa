using Atum.Domain.Basis;
using Atum.Domain.Common;
using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.QualityManagement.Auditing
{
    /// <summary>
    /// This class represents a Surveillance Performed
    /// </summary>
    public class Audit //: DomainObject
   {
        public Audit()
        {
        }

        public Audit(string surveyTitle, Person surveyor, int surveyId)
        {
            this.Surveyor = surveyor;
            this.SurveyId = surveyId;
        }


        public int Id { get; set; }

        public Person Surveyor { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateCompleted { get; set; }
        public Responses Responses { get; set; }
        public int SubcriberId { get; set; }
        public int Score { get; set; }
        public Surveillance Surveillance { get; set; }
        public int SurveyId { get; set; }
        public List<FollowUp> FollowUps { get; set; }
   }
}
