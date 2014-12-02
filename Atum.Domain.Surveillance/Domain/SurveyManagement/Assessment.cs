using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atum.Domain.Basis;

namespace Atum.Domain.SurveyManagement
{
    /// <summary>
    /// Do Not Use until analysed
    /// </summary>
    [Serializable]
    public class _Assessment : DomainObject
    {   
        public ManagedSurvey ConductedSurvey { get; set; }
        public Responses Responses { get; set; }

        public ManagedSurvey AdministeredSurvey { get; set; }
        public ManagedSurvey.QuestionEnumerator Enumerator { get; set; }

        public _Assessment()
        {   
        }

    }
}
