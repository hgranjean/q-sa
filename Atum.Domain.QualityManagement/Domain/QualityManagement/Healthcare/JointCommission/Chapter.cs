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

        public PerformanceCategory GetPerformanceCategory(string standardElementId)
        {
            
            int length = Elements.Count;

            for (int i = 0; i < length; i++)
            {
                if (Elements[i].StandardId.Equals(standardElementId))
                {
                    return Elements[i];
                }
            };

            return null;
        }
    }
}
