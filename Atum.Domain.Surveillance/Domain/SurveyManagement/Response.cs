using System;
using System.Collections.Generic;

namespace Atum.Domain.SurveyManagement
{
    [Serializable]
    public class Response
    {
        private Question _question;
        private ResponseChoice _answer;

        public Response()
        { }

        public Response(Question question, ResponseChoice answer)
        {
            this.QuestionId = (int)question.Id;
            this.AnswerKey = answer.Key;
            this.ResponseChoiceId = (int)answer.Id;
            this._answer = answer;
            this._question = question;
        }

        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int ResponseChoiceId { get; set; }
        public string AnswerKey { get; set; } // [aschmidt] Removed private set as it's a serializable class
        public string Text { get; set; }
        public List<string> FilesInfo { get; set; }

                
        // private Question Question { get; set; }
        //private ResponseChoice Answer { get; set; }
        
        public Question Question
        {
            get { return _question; }
            set { _question = value;  }
        }

        public ResponseChoice Answer
        {
            get { return _answer; }
            set { _answer = value; }
        }
    }
}
