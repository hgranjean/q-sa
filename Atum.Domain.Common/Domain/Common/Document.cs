using System;
using Atum.Domain.Basis;
using System;

namespace Atum.Domain.Common
{
    [Serializable]
    public class Document : DomainObject
	{
        public int OwnerId { get; set; }
        public long OwnerType { get; set; }
        public long StorageId { get; set; }

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
