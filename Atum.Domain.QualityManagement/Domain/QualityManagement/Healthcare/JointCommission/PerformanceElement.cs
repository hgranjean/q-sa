using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement.Healthcare.JointCommission
{
    public class PerformanceElement
    {
        public string Text { get; set; }
        public List<string> Notes { get; set; }

        public int EPId { get; set; }
    }
}
