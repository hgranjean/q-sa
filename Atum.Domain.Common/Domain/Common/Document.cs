using Atum.Domain.Basis;
using System;

namespace Atum.Domain.Common
{
    [Serializable]
    public class Document : DomainObject
    {
        public Document()
        {

        }

        //CTor
        public Document(int ownerId, long ownerType, long storageId)
        {
            OwnerId = ownerId;
            OwnerType = ownerType;
            StorageId = storageId;

        }

        //Props
        public int OwnerId { get; private set; }
        public long OwnerType { get; private set; }
        public long StorageId { get; private set; }


        protected override void setId(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}
