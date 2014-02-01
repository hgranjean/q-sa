using System;
using Atum.Domain.Basis;

namespace Atum.Domain.Billing
{
	/// <summary>
	/// Summary description for Insurance.
	/// </summary>
	[Serializable]public class Insurance : DomainObject
	{
		public Insurance()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        protected override void SetId(long id)
        {
            throw new NotImplementedException();
        }
    }
}
