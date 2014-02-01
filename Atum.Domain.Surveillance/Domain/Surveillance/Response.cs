using System;

namespace Atum.Domain.Surveillance
{
    [Serializable]
    public class Response
    {
        private ResponseChoice answer;

        public Response(Question question, ResponseChoice answer)
        {
            // TODO: Complete member initialization
            this.Question = question;
            this.answer = answer;
        }
        public Question Question { get; set; }
        public ResponseChoice Choice { get; set; }
    }
}
