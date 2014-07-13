using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.QualityManagement.Healthcare.JointCommission
{
    public class Chapter
    {
        public string Title { get; set; }
        public List<PerformanceCategory> Elements { get; set; }
    }
}
