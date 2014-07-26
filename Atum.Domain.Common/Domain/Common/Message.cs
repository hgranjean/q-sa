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
			//
		}

        protected override void SetId(long id)
        {
            throw new NotImplementedException();
        }
    }
}
