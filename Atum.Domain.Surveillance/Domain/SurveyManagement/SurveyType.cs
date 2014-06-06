using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.SurveyManagement
{
    [Serializable]
    public enum SurveyType
    {
        [Description("Evaluation")]
        Evaluation,

        [Description("Assessment")]
        Assessment,

        [Description("Auditing")]
        Audit, 
        
        [Description("Surveillance")]
        Surveillance
    }
}
