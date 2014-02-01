using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Domain.Surveillance
{
    [Serializable]
    public class SurveyStrategy
    {
        private DesignFeatures designFeatures;
        private Questions questions;
        private Stack<Question> questionsStack;

        public SurveyStrategy(DesignFeatures designFeatures)
        {
            // TODO: Complete member initialization
            this.designFeatures = designFeatures;
        }

        public SurveyStrategy(Questions questions)
        {
            // TODO: Complete member initialization
            this.questions = questions;
            LoadQuestionStack(questions);
        }

        private void LoadQuestionStack(Questions questions)
        {
            var orderedQuestions = new SortedList<int, Question>();
            questionsStack = new Stack<Question>();

            foreach (Question item in questions)
            {
                orderedQuestions.Add(item.Rank, item);
            }

            int length = orderedQuestions.Count;

            for (int i = length - 1; i >= 0; i--)
            {
                questionsStack.Push(orderedQuestions.Values[i]);
            }
        }

        public SurveyStrategy(QuestionStrategies qs)
        {
            // TODO: Complete member initialization
            this.QuestionStrategies = qs;
        }

        public Question NextQuestion { get; set; }

        internal Question GetNextQuestion(SurveyManager surveyManager)
        {
            //throw new NotImplementedException();
            //if the next available questions roots on any previous response then it 
            //should be returned.
            //Pop Question stack?
            Stack<Question> questions = this.questionsStack;

            if (questions != null && questions.Count > 0)
            {
                Question question = questions.Pop();
                while (!isValid(question, surveyManager))
                {
                    question = questions.Pop();
                }
                return question;

            }

            return null; // Nothing to return, null
        }

        private bool isValid(Question question, SurveyManager surveyManager)
        {
            //Need to qualify popped question per current manager state.
            return true;

        }

        public QuestionStrategies QuestionStrategies { get; set; }

    }
}
