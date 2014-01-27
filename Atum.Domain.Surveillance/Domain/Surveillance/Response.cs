using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Domain.Surveillance
{
 

    public class Response
    {
        private ResponseChoice answer;

        public Response(Surveillance.Question question, ResponseChoice answer)
        {
            // TODO: Complete member initialization
            this.Question = question;
            this.answer = answer;
        }
        public Question Question { get; set; }
        public ResponseChoice Choice { get; set; }
    }
}
