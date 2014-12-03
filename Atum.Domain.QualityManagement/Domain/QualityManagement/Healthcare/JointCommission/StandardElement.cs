using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement.Healthcare.JointCommission
{
    public class StandardElement
    {

        public string StandardId { get; set; }

        public string Content { get; set; }

        public IEnumerable<KeyValuePair<string, string>> EPIds { get; set; }

        public string Observation { get; set; }
    }
}
