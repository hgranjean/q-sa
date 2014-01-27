using Atum.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.Common
{
    public class TOCElementSpecification : ISpecification
    {
        public bool IsStatisfiedBy(Basis.DomainObject domainObject)
        {
            bool retVal = true;

            try
            {
                TOCElement element = (TOCElement)domainObject;

                retVal = element.Title != null ? element.Title != string.Empty : element.Title != null; 
            }
            catch (Exception)
            {

                throw;
            }


            return retVal;
        }
    }
}
