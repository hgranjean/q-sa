using System;

namespace Atum.Domain.Basis
{
	/// <summary>
	/// Summary description for DomainObject.
	/// </summary>
	public abstract class DomainObject
	{
		public long ID
		{
		    get;
		    set;
        }
        protected abstract void SetId(long id);
	}
}
