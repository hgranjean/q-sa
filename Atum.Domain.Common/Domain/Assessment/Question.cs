using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Domain.Assessment
{
    public class Question : Domain.Basis.DomainObject
    {
        private string description;
        private AnswerChoices responses;
        private ResponseType responseType;
        private Dictionary<string,AnswerChoice> answerChoices;

        public Question(int rank, string description, AnswerChoices responses, ResponseType responseType)
        {
            // TODO: Complete member initialization
            this.Rank = rank;
            this.description = description;
            this.Title = description;
            this.responses = responses;
            answerChoices = setAnswerChoices(responses);

            this.responseType = responseType;
        }

        private Dictionary<string, AnswerChoice> setAnswerChoices(AnswerChoices responses)
        {
            Dictionary<string,AnswerChoice> retVal = new Dictionary<string,AnswerChoice>();


            foreach (AnswerChoice item in responses)
            {
                retVal.Add(item.Response,item);
            }

            return retVal;
        }

        public AnswerChoice Answer { get; set; }

        public string Title { get; set; }

        public AnswerChoice SelectAnswer(string response)
        {
            try
            {
                return answerChoices[response];
            }
            catch (Exception)
            {
                
                throw new InvalidChoiceException();
            }
            
        }

        protected override void setId(long id)
        {
            throw new NotImplementedException();
        }

        public int Rank { get; private set; }
    }
}
