using System;

namespace Atum.Domain.Basis
{
	/// <summary>
	/// Summary description for DomainObject.
	/// </summary>
	public abstract class DomainObject
	{
	    public const int DefaultIdentifier = -1;

	    public long Id
		{
		    get;
		    set;
        }
        //protected abstract void SetId(long id);
	}
}
