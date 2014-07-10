using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atum.Domain.Basis;

namespace Atum.Domain.Common
{
    [Serializable]
    public class Company : DomainObject
    {
        public string DomainName { get; set; }
        public string CompanyName { get; set; }
        public string Industry { get; set; }

        public Company()
        {
            
        }

        protected override void SetId(long id)
        {
            throw new NotImplementedException();
        }
    }
}
