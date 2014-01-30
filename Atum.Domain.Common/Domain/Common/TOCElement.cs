using Atum.Domain.Basis;
using System;
namespace Atum.Domain.Common
{
    [Serializable]
    public class TOCElement : DomainObject
    {

        public TOCElement(string elementTitle)
        {
            // TODO: Complete member initialization
            Title = elementTitle;
        }



        public string Title { get; set; }
        public int Level { get; set; }
        public string Content { get; set; }
        public string ShortContent { get; set; }

        protected override void setId(long id)
        {
            throw new System.NotImplementedException();
        }
    }

}
