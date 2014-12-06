namespace Atum.Domain.Common
{
    using Atum.Domain.Specification;
    using System;
    using System.Collections.Generic;


    [Serializable]
    public class TableOfContents
    {
        Dictionary<string, TOCElement> elementsByTitle;// = new Dictionary<string, TOCElement>();    

        public TOCElements TOCElements { get; set; }

        public TOCElement GetElementByTitle(string title)
        {
            TOCElement retVal = null;
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



        public TOCElement AddElement(TOCElement tocElement)
        {
            if (TOCElements == null)
            {
                TOCElements = new TOCElements();
            }
            ISpecification TOCElementSpecification = new TOCElementSpecification();
            if (TOCElementSpecification.IsStatisfiedBy(tocElement))
            {
                TOCElements.Add(tocElement);
                this.FindersAdd(tocElement);

            }
            return tocElement;
        }

        public TOCElement AddElement(string elementTitle)
        {
            TOCElement tocElement = new TOCElement(elementTitle);
            return AddElement(tocElement);

        }

        private void FindersAdd(TOCElement tocElement)
        {
            //ElementByTitle
            if (elementsByTitle==null)
            {
                elementsByTitle = new Dictionary<string, TOCElement>();
            }
            
            if (!elementsByTitle.ContainsKey(tocElement.Title))
            {
                elementsByTitle.Add(tocElement.Title,tocElement);
            };

        }

    } 
  
}
