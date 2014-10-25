using Atum.Domain.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.SurveyManagement
{
    public class Audit : DomainObject
   {
        public Audit()
        {

        }

        public Audit(string surveyTitle)
        {
            this.SurveyTitle = surveyTitle;
        }


        public string SurveyTitle { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime DateCompleted { get; set; }
        public Responses Responses { get; set; }
        public int SubcriberId { get; set; }
        public int Score { get; set; }

   }
}
