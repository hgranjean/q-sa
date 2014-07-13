using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement.Healthcare.JointCommission
{
    public class PerformanceCategory
    {
        public string EPID { get; set; }
        public string Title { get; set; }
        public List<Item> Items { get; set; }
    }
}
