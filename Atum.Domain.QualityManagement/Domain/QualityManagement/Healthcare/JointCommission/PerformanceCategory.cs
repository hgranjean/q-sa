using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement.Healthcare.JointCommission
{
    public class PerformanceCategory
    {
        public string StandardId { get; set; }
        public string Title { get; set; }
        public List<Item> Items { get; set; }
    }
}
