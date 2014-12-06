using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement.Healthcare.Performance
{
    public abstract class xElement
    {
        public xElement(string key, string title)
        {
            // TODO: Complete member initialization
            this.Title = title;
            this.Key = key;
        }

        public string Title { get; set; }
        public string Key { get; set; }
        public string Content { get; set; }
        public IEnumerable<xElement> Elements { get; set; }
    }
}
