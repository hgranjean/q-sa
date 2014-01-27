using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Domain.Basis
{
    interface IDomainObjectFactory
    {
        DomainObject GetNew();
    }
}
