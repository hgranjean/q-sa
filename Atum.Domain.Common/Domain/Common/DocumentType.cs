
using Atum.Domain.Basis;
using System;

namespace Atum.Domain.Common
{
    [Serializable]
    public class DocumentType : DomainObject
    {
        //Props
        public int Value { get; private set; }

        public DocumentType(int value)
        {
            Value = value;
        }

        protected override void SetId(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}
