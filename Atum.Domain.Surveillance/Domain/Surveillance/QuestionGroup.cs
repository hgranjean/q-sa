using System;

namespace Atum.Domain.Surveillance
{
    [Serializable]
    public class QuestionGroup
    {
        private String GroupTitle { get; set; }
        public int Number { get; set; }

        protected QuestionGroup()
        {
        }

        public QuestionGroup(string groupTitle)
        {
            // TODO: Complete member initialization
            this.GroupTitle = groupTitle;
        }
        public Questions Questions { get; set; }

        public Question AddQuestion(string questionText)
        {
            return AddQuestion(questionText, QuestionType.OpenVariant);
        }

        public Question AddQuestion(string questionText, QuestionType qType)
        {
            var retVal = new Question(questionText, qType);
            retVal = setTypeDefaults(retVal);
            EnsureQuestions();
            
            Questions.Add(retVal);
            retVal.Number = Questions.Count;

            return retVal;
        }

        private void EnsureQuestions()
        {
            if (Questions == null)
            {
                Questions = new Questions();
            }
        }

        private Question setTypeDefaults(Question question)
        {
            var questionType = question.QuestionType;
            switch (questionType)
            {
                case QuestionType.YesNo:
                    question.AddChoice("Yes");
                    question.AddChoice("No");
                    break;
                case QuestionType.TrueFalse:
                    question.AddChoice("True");
                    question.AddChoice("False");
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
            }
            return question;
        }
    }
}
