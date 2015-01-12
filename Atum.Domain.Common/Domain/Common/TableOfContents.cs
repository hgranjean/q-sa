namespace Atum.Domain.Common
{
    using Atum.Domain.Specification;
    using System;
    using System.Collections.Generic;


    [Serializable]
    public class TableOfContents
    {

        Dictionary<string, DocumentElement> elementsByTitle;// = new Dictionary<string, TOCElement>();    
        Dictionary<string, DocumentElement> elementsByKey;// = new Dictionary<string, TOCElement>();    

        public List<DocumentElement> TOCElements { get; set; }

        public DocumentElement GetElementByTitle(string title)
        {
            DocumentElement retVal = null;
            try
            {
                retVal = elementsByTitle[title];
            }
            catch (Exception)
            {
                
                //throw;
                throw new Atum.Domain.Common.TOCElementNotFoundException();
            }
            return retVal;
        }

        public DocumentElement GetElementByKey(string key)
        {
            DocumentElement retVal = null;
            try
            {
                retVal = elementsByKey[key];
            }
            catch (Exception)
            {

                //throw;
                throw new Atum.Domain.Common.TOCElementNotFoundException();
            }
            return retVal;
        
        }

        public DocumentElement AddElement(DocumentElement tocElement)
        {
            if (TOCElements == null)
            {
                TOCElements = new DocumentElements();
            }
            //ISpecification TOCElementSpecification = new TOCElementSpecification();
            //if (TOCElementSpecification.IsStatisfiedBy(tocElement))
            //{
                TOCElements.Add(tocElement);
                this.FindersAdd(tocElement);

            //}
            return tocElement;
        }

        //public DocumentElement AddElement(string elementKey, string elementTitle)
        //{
        //    DocumentElement tocElement = new DocumentElement(elementKey, elementTitle);
        //    return AddElement(tocElement);

        //}

        private void FindersAdd(DocumentElement tocElement)
        {
            //ElementByTitle
            if (elementsByTitle==null)
            {
                elementsByTitle = new Dictionary<string, DocumentElement>();
                elementsByKey = new Dictionary<string, DocumentElement>();
            }
            
            if (!elementsByTitle.ContainsKey(tocElement.Title))
            {
                elementsByTitle.Add(tocElement.Title,tocElement);
            };

            if (!elementsByKey.ContainsKey(tocElement.Title))
            {
                elementsByKey.Add(tocElement.Key, tocElement);
            };
        }

    } 
  
}
