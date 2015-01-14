using Atum.Domain.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.QualityManagement.Healthcare.Performance
{
    public class ItemNote //: DomainObject
    {

        public ItemNote(string note)
        {
            // TODO: Complete member initialization
            this.Text = note;
        }

        public Guid ItemNodeId { get; set; }
        public string Text { get; set; }
    }
}
