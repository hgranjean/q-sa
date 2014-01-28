using System;

using Atum.Domain.Basis;

namespace Atum.Domain.Clinical
{
	/// <summary>
	/// Summary description for Lab.
	/// </summary>
	public class Lab : DomainObject
	{
		public Lab()
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
