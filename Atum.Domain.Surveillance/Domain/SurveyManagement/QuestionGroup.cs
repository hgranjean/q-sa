using System;

namespace Atum.Domain.SurveyManagement
{
    [Serializable]
    public class QuestionGroup
    {
        public String Title { get; set; }
        public int Number { get; set; }

        public QuestionGroup()
        {
        }

        public QuestionGroup(string groupTitle)
        {
            // TODO: Complete member initialization
            this.Title = groupTitle;
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

        public Question AddQuestion(string questionNumber, string questionText, QuestionType questionType)
        {
            var retVal = AddQuestion(questionText, questionType);
            retVal.Label = questionNumber;
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
