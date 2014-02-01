using System;
using Atum.Domain.Basis;
using System;

namespace Atum.Domain.Common
{
    [Serializable]
    public class Document : DomainObject
	{
        //Props
        public int OwnerId { get; private set; }
        public long OwnerType { get; private set; }
        public long StorageId { get; private set; }

        public Document()
        {
        }

		public Document (int ownerId,long ownerType,long storageId)
		{
			OwnerId = ownerId;
			OwnerType = ownerType;
			StorageId = storageId;
		}

        protected override void SetId(long id)
        {
            throw new NotImplementedException();
        }
    }
}
