using System;

using Atum.Domain.Basis;

namespace Atum.Domain.Clinical
{
	/// <summary>
	/// Summary description for Patient.
	/// </summary>
	public class Patient :	Atum.Domain.Common.Person
	{
        private long _addressId;
		public Patient()
		{
		}
        public long AddressId { get { return _addressId; } set { _addressId = value; } }
    }
}
