using System;

using Atum.Domain.Basis;

namespace Atum.Domain.Billing
{
	/// <summary>
	/// Summary description for FeeSchedule.
	/// </summary>
	public class FeeSchedule : DomainObject
	{
		public FeeSchedule()
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
