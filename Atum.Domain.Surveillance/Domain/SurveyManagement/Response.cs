using System;

namespace Atum.Domain.SurveyManagement
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
