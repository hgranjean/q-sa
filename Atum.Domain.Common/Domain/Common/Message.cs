using System;
using Atum.Domain.Basis;

namespace Atum.Domain.Common
{
	/// <summary>
	/// Summary description for Message.
	/// </summary>
	[Serializable]public class Message : DomainObject
	{
		public Message()
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
