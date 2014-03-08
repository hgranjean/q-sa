using System.Collections.Generic;

using System;
using Atum.Domain.Basis;

namespace Atum.Domain.Surveillance
{
    [Serializable]
    public class ResponseChoice : DomainObject
    {
        protected ResponseChoice()
        {
        }

        public ResponseChoice(string choiceText)
        {
            // TODO: Complete member initialization
            this.Text = choiceText;
        }
        public string Text { get; set; }

        protected override void SetId(long id)
        {
            throw new NotImplementedException();
        }

        public void SetIdInternal(long id)
        {
            ID = id;
        }
    }
}
