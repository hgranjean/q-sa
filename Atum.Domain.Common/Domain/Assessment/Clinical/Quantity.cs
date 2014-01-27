using System;

using Atum.Domain.Basis;

namespace Atum.Domain.Clinical
{
	/// <summary>
	/// Summary description for Quantity.
	/// </summary>
	public class Quantity : DomainObject
	{
		public Quantity()
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
