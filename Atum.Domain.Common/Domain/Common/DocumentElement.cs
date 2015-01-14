using Atum.Domain.Basis;
using System;
using System.Text;
namespace Atum.Domain.Common
{
    [Serializable]
    public abstract class DocumentElement //: DomainObject
    {
        StringBuilder sb = new StringBuilder();

        public DocumentElement(string itemKey, string elementTitle)
        {
            Title = elementTitle;
            Key = itemKey;
        }
        public void AddContent(string text)
        {
            sb.Append(text);
        }
        public void AddContentLine(string text)
        {
            sb.AppendLine(text);
        }

        public string Title { get; set; }
        public string Key { get; set; }
        public int Level { get; set; }
        public string Content {get { return sb.ToString(); }}
        public string ShortContent { get; set; }

    }

}
