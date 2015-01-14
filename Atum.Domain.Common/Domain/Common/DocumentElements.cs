
using System;
using System.Collections.Generic;
namespace Atum.Domain.Common
{
    [Serializable]
    public class DocumentElements : List<DocumentElement>
    {
        public TableOfContents TableOfContents { get; set; }

        new public void Add(DocumentElement docItem)
        {
            base.Add(docItem);
            if (TableOfContents==null)
            {
                TableOfContents = new TableOfContents();
            }
            TableOfContents.AddElement(docItem);

        }

    }
}
