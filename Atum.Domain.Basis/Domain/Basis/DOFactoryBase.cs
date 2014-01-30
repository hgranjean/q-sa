

using Atum.Domain.Specification;

namespace Atum.Domain.Basis
{
   public abstract class DOFactoryBase
	{
       ISpecification _spec;

       protected DOFactoryBase(ISpecification spec) 
       {
           _spec = spec;
       }

       protected bool isValid(DomainObject dO)
       {
           return _spec.IsStatisfiedBy(dO);
       }
       
	}
}
