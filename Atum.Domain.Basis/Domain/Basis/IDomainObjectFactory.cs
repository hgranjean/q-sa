using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Domain.Basis
{
    public interface IDomainObjectFactory
    {
        DomainObject GetNew();
    }
}
