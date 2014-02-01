using System;
using Atum.Domain.Basis;

namespace Atum.Domain.Common
{
	/// <summary>
	/// Summary description for Pharmacy.
	/// </summary>
	[Serializable]public class Pharmacy : DomainObject
	{
		public Pharmacy()
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
