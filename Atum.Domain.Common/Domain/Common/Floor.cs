using Atum.Domain.Basis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.Common
{
    [Serializable]
    public class Floor : DomainObject
    {
        protected override void SetId(long id)
        {
            throw new NotImplementedException();
        }
    }
}
