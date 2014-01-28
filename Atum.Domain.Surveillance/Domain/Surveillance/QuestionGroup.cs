using System;

namespace Atum.Domain.Surveillance
{
    [Serializable]
    public class QuestionGroup
    {
        private string groupTittle;

        public QuestionGroup(string groupTittle)
        {
            // TODO: Complete member initialization
            this.groupTittle = groupTittle;
        }
        public Questions Questions { get; set; }

        public Question AddQuestion(string questionText)
        {
            return AddQuestion(questionText, QuestionType.OpenVariant);
        }

        public Question AddQuestion(string questionText, QuestionType qType)
        {
            Question retVal = new Question(questionText, qType);
            retVal = setTypeDefaults(retVal);
            if (Questions == null)
            {
                Questions = new Questions();
            }
            Questions.Add(retVal);
            retVal.Number = Questions.Count;

            return retVal;
        }

        private Question setTypeDefaults(Question question)
        {
            QuestionType qType = question.QuestionType;
            switch (qType)
            {
                case QuestionType.YesNo:
                    ResponseChoice choice = question.AddChoice("Yes");
                    choice = question.AddChoice("No");
                    break;
                case QuestionType.TrueFalse:
                    choice = question.AddChoice("True");
                    choice = question.AddChoice("False");
                    break;
                case QuestionType.SelectOne:
                    break;
                case QuestionType.SelectMultiple:
                    break;
                case QuestionType.SelectOneConditional:
                    break;
                case QuestionType.OpenText:
                    break;
                case QuestionType.OpenVariant:
                    break;
                case QuestionType.Ranking:
                    break;
                default:
                    break;
            }
            return question;
        }

        public int Number { get; set; }
    }
}
