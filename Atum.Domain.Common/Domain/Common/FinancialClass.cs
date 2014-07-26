using System;
using Atum.Domain.Basis;

namespace Atum.Domain.Common
{
	/// <summary>
	/// Summary description for FinancialClass.
	/// </summary>
	[Serializable]public class FinancialClass : DomainObject
	{
		public FinancialClass()
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
