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
            this.Standards = new DocumentElements();
        }

        public Standard GetPerformanceCategory(string standardElementId)
        {
            throw new NotImplementedException();
        }

        public Guid ChapterId { get; set; }
        public DocumentElements Standards { get; set; }

        public Standard AddStandard(string standardKey, string standardTitle)
        {
            Standard standard = new Standard(standardKey, standardTitle);
            Standards.Add(standard);
            return standard;
        }

    }
}
