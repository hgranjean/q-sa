using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement.Healthcare.JointCommission
{
    public class Standard
    {
        public string StandardId { get; set; }
        public string Title { get; set; }
        public List<Chapter> Chapters { get; set; }
        public List<ElementOfPerformance> Items { get; set; }
        public int MyProperty { get; set; }
    }
}
