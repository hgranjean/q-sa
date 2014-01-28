using System;

namespace Atum.Domain.Common
{
	/// <summary>
	/// Summary description for Contact.
	/// </summary>
	public class Contact : Domain.Basis.DomainObject
	{
		public Contact()
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
