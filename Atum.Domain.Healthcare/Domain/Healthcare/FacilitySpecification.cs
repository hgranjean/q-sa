using Atum.Domain.Basis;
using Atum.Domain.Specification;
using System;

namespace Atum.Domain.Healthcare
{
    [Serializable]
    public class FacilitySpecification: ISpecification
    {
        public bool IsStatisfiedBy(DomainObject domainObject)
        {
            throw new NotImplementedException();
        }
    }
}
