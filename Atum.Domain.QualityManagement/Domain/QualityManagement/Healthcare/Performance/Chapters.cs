using Atum.Domain.Common;
using System;
using System.Collections.Generic;


namespace Atum.Domain.QualityManagement.Healthcare.Performance
{
    [Serializable]
    public class Chapters : List<Chapter>
    {
        public TableOfContents TableOfContents { get; set; }

        new public void Add(Chapter docItem)
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
