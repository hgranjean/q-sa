using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Atum.Domain.Basis;
using Atum.Domain.Common;

namespace Atum.Domain.SurveyManagement
{
    [Serializable]    
    public class Question : DomainObject
    {
        // Needed for serialization
        public Question()
        {}

        public Question(string questionText, QuestionType qType, QuestionGroup qGroup, string referenceElementId)
        {
            this.Text = questionText;
            this.QuestionType = qType;
            this.PerformanceItemId = referenceElementId;
            //TODO: Resolve null group
            if (qGroup != null)
            {
                this.QuestionGroup = qGroup;
                this.GroupNumber = qGroup.Number;

            }
        }


        public int Rank { get; set; }

        public string Text { get; set; }
        
        public int Number { get; set; }
        
        public string Label { get; set; }
        
        public int GroupNumber { get; set; }
        
        public QuestionType QuestionType { get; set; }
    
        public ResponseChoices ResponseChoices { get; set; }
        
        public DocumentElement PerformanceItem { get; set; }
        public string PerformanceItemId { get; set; }

        public QuestionGroup QuestionGroup { get; set; }




        public ResponseChoice AddChoice(string choiceText)
        {
            var retVal = new ResponseChoice(choiceText);

            if (ResponseChoices == null)
            {
                ResponseChoices = new ResponseChoices();
            }

            if (AddChoiceAllowed())
            {
                ResponseChoices.Add(retVal);
                FindersAdd(retVal);
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
            }
            return retVal;
        }

        private Dictionary<string, ResponseChoice> choicesByText = new Dictionary<string, ResponseChoice>();
        public ResponseChoice GetResponseByText(string choiceText)
        {
            ResponseChoice retVal = null;
            if (!choicesByText.TryGetValue(choiceText, out retVal))
            {
                throw new TOCElementNotFoundException();
            }
            
            return retVal;
        }

        public ResponseChoice AddConditionalChoice(string choiceText, ResponseChoice conditionOnChoice)
        {
            throw new NotImplementedException();
        }





    }
}
