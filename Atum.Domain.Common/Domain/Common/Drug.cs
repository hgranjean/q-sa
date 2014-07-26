using System;
using Atum.Domain.Basis;

namespace Atum.Domain.Common
{
	/// <summary>
	/// Summary description for Drug.
	/// </summary>
	[Serializable]public class Drug : DomainObject
	{
		public Drug()
		{
			//
			//
		}

        protected override void SetId(long id)
        {
            throw new NotImplementedException();
        }
    }
}
