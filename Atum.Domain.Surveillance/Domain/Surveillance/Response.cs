using System;

namespace Atum.Domain.Surveillance
{
    [Serializable]
    public class Response
    {
        private ResponseChoice _answer;

        public Response(Question question, ResponseChoice answer)
        {
            // TODO: Complete member initialization
            this.Question = question;
            this.Answer = answer;
        }
        public Question Question { get; private set; }
        public ResponseChoice Answer { get; private set; }
    }
}
