using System;
using System.Collections.Generic;
using System.Text;

using Atum.Domain.Basis;
using Atum.Domain.Clinical;

namespace Atum.Domain.Specification
{
    public class PatientSpecification : ISpecification
    {
        #region ISpecification Members

        public bool IsStatisfiedBy(DomainObject domainObject)
        {
            Patient patient = (Patient)domainObject;
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
