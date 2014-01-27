using System;
using Atum.Domain.Basis;

namespace Atum.Domain.Common
{
	/// <summary>
	/// Summary description for Credential.
	/// </summary>
	public class Credential : DomainObject
	{
		public Credential()
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
