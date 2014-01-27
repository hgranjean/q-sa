using System;
using Atum.Domain.Basis;

namespace Atum.Domain.Specification
{
	/// <summary>
	/// Summary description for Specification.
	/// </summary>
	public interface ISpecification
	{
		bool IsStatisfiedBy(DomainObject domainObject);
	}
}
