using System;

namespace Atum.Domain.SurveyManagement.Specifications
{
    [Serializable]public class SurveySpecification : Atum.Domain.Specification.ISpecification
    {
        public bool IsStatisfiedBy(Basis.DomainObject domainObject)
        {
            throw new NotImplementedException();
        }
    }
}
