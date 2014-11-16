using System;
using System.Collections.Generic;

namespace Atum.Domain.SurveyManagement
{
    [Serializable]
    public class Response
    {   
        public Response()
        { }

        public Response(Question question, ResponseChoice answer)
        {
            QuestionId = (int)question.Id;
            AnswerKey = answer.Key;
            ResponseChoiceId = (int)answer.Id;
            Answer = answer;
            Question = question;
        }

        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int ResponseChoiceId { get; set; }
        public string AnswerKey { get; set; } // [aschmidt] Removed private set as it's a serializable class
        public string Text { get; set; }
        public List<string> FilesInfo { get; set; }

                
        // private Question Question { get; set; }
        //private ResponseChoice Answer { get; set; }
        
        public Question Question { get; set; }
        public ResponseChoice Answer { get; set; }
    }
}
