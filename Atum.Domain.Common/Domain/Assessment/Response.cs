using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Domain.Assessment
{
    public enum ResponseType
    {
        SelectOne,
        SelectMultiple,
        LongAnswer,
        ShortAnswer
    }

    public class Response
    {
        private AnswerChoice answer;

        public Response(Assessment.Question question, AnswerChoice answer)
        {
            // TODO: Complete member initialization
            this.Question = question;
            this.answer = answer;
        }
        public Question Question { get; set; }
        public AnswerChoice Choice { get; set; }
    }
}
