
using Atum.Domain.Basis;

namespace Atum.Domain.Common
{
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
