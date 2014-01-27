using System;

namespace Atum.Utility.Logic
{
	/// <summary>
	/// Summary description for Specification.
	/// </summary>
	public interface ISpecification
	{
		bool IsStatisfiedBy(object domainObject);
	}
}
