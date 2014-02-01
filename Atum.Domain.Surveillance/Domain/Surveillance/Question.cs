using System;
using System.Collections.Generic;

namespace Atum.Domain.Surveillance
{
    [Serializable]
    public class Question : Domain.Basis.DomainObject
    {

        public Question(string questionText, Surveillance.QuestionType qType)
        {
            // TODO: Complete member initialization
            this.Text = questionText;
            this.QuestionType = qType;
        }


        public string Text { get; set; }
        public int Number { get; set; }
        public int Rank { get; set; }
        public Common.TOCElement BasisReference { get; set; }
        public QuestionType QuestionType { get; set; }
        public ResponseChoices ResponseChoices { get; set; }

        protected override void SetId(long id)
        {
            throw new NotImplementedException();
        }

        public ResponseChoice AddChoice(string choiceText)
        {
            ResponseChoice retVal = null;
            try
            {
                retVal = new ResponseChoice(choiceText);

                if (ResponseChoices == null)
                {
                    ResponseChoices = new ResponseChoices();
                }

                if (this.AddChoiceAllowed())
                {
                    ResponseChoices.Add(retVal);
                    this.FindersAdd(retVal);

                }
            }
            catch (Exception)
            {

                throw;
            }

            return retVal;

        }

        private void FindersAdd(ResponseChoice choice)
        {
            //ElementByTitle
            if (choicesByText == null)
            {
                choicesByText = new Dictionary<string, ResponseChoice>();
            }

            if (!choicesByText.ContainsKey(choice.Text))
            {
                choicesByText.Add(choice.Text, choice);
            };

        }

        private bool AddChoiceAllowed()
        {
            bool retVal = true;
            switch (this.QuestionType)
            {
                case QuestionType.YesNo:
                    retVal = this.ResponseChoices.Count < 2;
                    break;
                case QuestionType.TrueFalse:
                    retVal = this.ResponseChoices.Count < 2;
                    break;
                case QuestionType.SelectOne:
                    break;
                case QuestionType.SelectMultiple:
                    break;
                case QuestionType.YesNoConditional:
                    retVal = this.ResponseChoices.Count < 3;
                    break;
                case QuestionType.TrueFalseConditional:
                    retVal = this.ResponseChoices.Count < 3;
                    break;
                case QuestionType.SelectOneConditional:
                    break;
                case QuestionType.OpenText:
                    retVal = this.ResponseChoices.Count < 1;
                    break;
                case QuestionType.OpenVariant:
                    break;
                case QuestionType.Ranking:
                    break;
                default:
                    break;
            }
            return retVal;
        }

        private Dictionary<string, ResponseChoice> choicesByText = new Dictionary<string, ResponseChoice>();
        public ResponseChoice GetResponseByText(string choiceText)
        {
            ResponseChoice retVal = null;
            try
            {
                retVal = choicesByText[choiceText];
            }
            catch (Exception)
            {

                //throw;
                throw new Atum.Domain.Common.TOCElementNotFoundException();
            }
            return retVal;
        }

        public ResponseChoice AddConditionalChoice(string choiceText, ResponseChoice conditionOnChoice)
        {
            throw new NotImplementedException();
        }
    }
}
