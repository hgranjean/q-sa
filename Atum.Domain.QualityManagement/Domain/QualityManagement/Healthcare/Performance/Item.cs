using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement.Healthcare.Performance
{
    public class Item
    {
        private string itemKey;
        private string itemTitle;

        public Item(string itemKey, string itemTitle)
        {
            // TODO: Complete member initialization
            this.itemKey = itemKey;
            this.itemTitle = itemTitle;
        }

        public List<string> RelatedItemKeys { get; set; }
        
        public string Text { get; set; }
        
        public List<string> Notes { get; set; }

        public int EPId { get; set; }
    }
}
