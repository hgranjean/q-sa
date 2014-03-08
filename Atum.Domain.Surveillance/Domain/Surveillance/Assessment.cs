using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atum.Domain.Basis;

namespace Atum.Domain.Surveillance
{
    [Serializable]
    public class Assessment : DomainObject
    {   
        public Survey ConductedSurvey { get; set; }
        public Responses Responses { get; set; }

        public Survey AdministeredSurvey { get; set; }
        public Survey.QuestionEnumerator Enumerator { get; set; }
        // public SurveyResponse Responses { get; set; }

        public Assessment()
        {   
        }

        protected override void SetId(long id)
        {
            throw new NotImplementedException();
        }
    }
}
