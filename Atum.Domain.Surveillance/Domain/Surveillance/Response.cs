using System;

namespace Atum.Domain.Surveillance
{
    [Serializable]
    public class Response
    {
        public Response()
        {
            
        }

        public Response(Question question, ResponseChoice answer)
        {
            this.Question = question;
            this.Answer = answer;
        }
        public Question Question { get; set; }
        public ResponseChoice Answer { get; set; }
    }
}
