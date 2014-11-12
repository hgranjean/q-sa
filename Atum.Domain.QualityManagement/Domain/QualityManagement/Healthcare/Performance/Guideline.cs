using Atum.Domain.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement.Healthcare.Performance
{
    public class Guideline : DomainObject
    {
        public Guideline()
        {
        }

        public Guideline(string title)
        {
            this.Title = title;
        }


        public string Title { get; set; }
        public List<Document> Documents { get; set; }
        public List<Element> Elements { get; set; }
        public int OwnerId { get; set; }
    }
}
