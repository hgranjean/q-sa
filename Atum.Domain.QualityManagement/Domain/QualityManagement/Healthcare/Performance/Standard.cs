using Atum.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement.Healthcare.Performance
{
    /// <summary>
    /// 
    /// </summary>
    public class Standard : DocumentElement
    {

        public Standard():base("","") 
        {
        }

        public Standard(string key, string title):base(key,title)
        {
            this.PerformanceItems = new PerformanceItems();
        }

        public Guid StandardId { get; set; }
        public Guid ChapterId { get; set; }
        public Chapter Chapter { get; set; }
        public virtual PerformanceItems PerformanceItems { get; set; }

        public PerformanceItem AddPerformanceItem(string itemKey, string itemTitle)
        {
            PerformanceItem performanceItem = new PerformanceItem(itemKey, itemTitle);
            PerformanceItems.Add(performanceItem);
            return performanceItem;
        }

    }
}
