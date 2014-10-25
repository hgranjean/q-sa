using Atum.Domain.Basis;
using System;
using System.Collections.Generic;

namespace Atum.Domain.SurveyManagement
{
    [Serializable]
    public class ResponseChoice : DomainObject
    {   
        protected ResponseChoice()
        {
        }

        public ResponseChoice(string choiceText)
        {
            this.Text = choiceText;
        }
        public string Text { get; set; }

        public string Value { get; set; }

        public string Number { get; set; }

        public string Key { get; set; }


        public void SetIdInternal(long id)
        {
            Id = id;
        }
    }
}
