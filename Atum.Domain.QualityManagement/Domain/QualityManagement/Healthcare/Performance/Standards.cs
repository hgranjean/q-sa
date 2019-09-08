using Atum.Domain.Common;
using System;
using System.Collections.Generic;


namespace Atum.Domain.QualityManagement.Healthcare.Performance
{
    [Serializable]
    public class Standards : List<Standard>
    {
        public TableOfContents TableOfContents { get; set; }

        new public void Add(Standard docItem)
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
