using System;
using Atum.Domain.Basis;

namespace Atum.Domain.Common
{
	/// <summary>
	/// Summary description for Ethnicity.
	/// </summary>
	[Serializable]public class Ethnicity : DomainObject
	{
		public Ethnicity()
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
