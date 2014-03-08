using System;
using Atum.Domain.Specification;

namespace Atum.Domain.Healthcare
{
    [Serializable]
    public class FacilitySpecification: ISpecification
    {
        

        public bool IsStatisfiedBy(Basis.DomainObject domainObject)
        {
            throw new NotImplementedException();
        }
    }
}
