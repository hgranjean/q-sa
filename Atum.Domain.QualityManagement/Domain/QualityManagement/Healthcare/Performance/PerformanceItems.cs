using Atum.Domain.Common;
using System;
using System.Collections.Generic;


namespace Atum.Domain.QualityManagement.Healthcare.Performance
{
    [Serializable]
    public class PerformanceItems : List<PerformanceItem>
    {
        public TableOfContents TableOfContents { get; set; }

        new public void Add(PerformanceItem docItem)
        {
            base.Add(docItem);
            if (TableOfContents == null)
            {
                TableOfContents = new TableOfContents();
            }
            TableOfContents.AddElement(docItem);

        }

    }
}
