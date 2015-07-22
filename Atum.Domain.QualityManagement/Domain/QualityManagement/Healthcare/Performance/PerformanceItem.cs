using Atum.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement.Healthcare.Performance
{
    public class PerformanceItem : DocumentElement
    {

        public PerformanceItem(string itemKey, string itemTitle):base(itemKey,itemTitle)
        {
        }

        public Guid PerformanceItemId { get; set; }

        public Guid StandardId { get; set; }
        public Standard Standard { get; set; }
        public virtual List<string> RelatedItemKeys { get; set; }
        public virtual ItemNotes Notes { get; set; }
        
        public string Text { get; set; }
        public int EPId { get; set; }

    }
}
