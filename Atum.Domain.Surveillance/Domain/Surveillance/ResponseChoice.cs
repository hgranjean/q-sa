using System.Collections.Generic;

using System;
namespace Atum.Domain.Surveillance
{
    [Serializable]
    public class ResponseChoice
    {

        public ResponseChoice(string choiceText)
        {
            // TODO: Complete member initialization
            this.Text = choiceText;
        }
        public string Text { get; set; }
    }
}
