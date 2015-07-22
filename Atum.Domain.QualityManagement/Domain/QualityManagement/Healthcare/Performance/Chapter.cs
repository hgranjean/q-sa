using Atum.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.QualityManagement.Healthcare.Performance
{
    public class Chapter : DocumentElement
    {

        public Chapter(string key, string title):base(key,  title)
        {
            this.Standards = new Standards();

        }

        public long StandardDocumentId { get; set; }
        public StandardDocument StandardDocument { get; set; }

        public Standard GetPerformanceCategory(string standardElementId)
        {
            throw new NotImplementedException();
        }

        public Guid ChapterId { get; set; }
        public virtual Standards Standards { get; set; }

        public Standard AddStandard(string standardKey, string standardTitle)
        {
            Standard standard = new Standard(standardKey, standardTitle);
            Standards.Add(standard);
            return standard;
        }


    }
}
