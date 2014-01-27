using System;

using Atum.Domain.Basis;

namespace Atum.Domain.Billing
{
	/// <summary>
	/// Summary description for Coverage.
	/// </summary>
	public class Coverage : DomainObject
	{
		public Coverage()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        protected override void setId(long id)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
