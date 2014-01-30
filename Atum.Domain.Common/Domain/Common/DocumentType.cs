
using Atum.Domain.Basis;
using System;

namespace Atum.Domain.Common
{
    [Serializable]
    public class DocumentType : DomainObject
    {

        //CTor
        public DocumentType(int value)
        {
            Value = value;

        }

        //Props
        public int Value { get; private set; }


        protected override void setId(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}
