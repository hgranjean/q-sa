using Atum.Domain.Basis;
using System;
namespace Atum.Domain.Common
{
    [Serializable]
    public class TOCElement : DomainObject
    {
        public static readonly TOCElement None = new TOCElement();

        protected TOCElement()
        {
        }

        public TOCElement(string elementTitle)
        {
            Title = elementTitle;
        }

        public string Title { get; set; }
        public int Level { get; set; }
        public string[] Content { get; set; }
        public string ShortContent { get; set; }

        protected override void SetId(long id)
        {
            throw new System.NotImplementedException();
        }
    }

}
