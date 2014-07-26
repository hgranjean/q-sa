using System;
using Atum.Domain.Basis;

namespace Atum.Domain.Common
{
	/// <summary>
	/// Summary description for Note.
	/// </summary>
	[Serializable]public class Note : DomainObject
	{
		public Note()
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
