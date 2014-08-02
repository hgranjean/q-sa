using Atum.Domain.QualityManagement;
using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.Domain.QualityManagement.Healthcare
{
    public class FollowUp
    {
        public string Status { get; set; }
        public DateTime InitialDueDate { get; set; }
        public Question Question { get; set; }
        public Observation Observation { get; set; }

    }
}
