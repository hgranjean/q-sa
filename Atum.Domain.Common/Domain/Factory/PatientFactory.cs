using System;
using System.Collections.Generic;
using System.Text;

using Atum.Domain.Basis;
using Atum.Domain.Clinical;
using Atum.Domain.Specification;

namespace Atum.Domain.Factory
{
    public sealed class PatientFactory : DOFactoryBase, IDomainObjectFactory
    {
        public PatientFactory(ISpecification spec)
            : base(spec)
       { }

        private Patient GetPatient()
        {
            Patient retVal = new Patient();
            if(!isValid(retVal))
            {
                throw new SpecificationNotSatisfiedException();
            }
            return retVal;
        }

        #region IDomainObjectFactory Members

        public DomainObject GetNew()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
