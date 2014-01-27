using System;

using Atum.Domain.Basis;

namespace Atum.Domain.Billing
{
	/// <summary>
	/// Summary description for Fee.
	/// </summary>
	public class Fee : DomainObject
	{
		public Fee()
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
