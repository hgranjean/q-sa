using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Domain.Assessment
{
    /// <summary>
    /// Survey - Survey is the set of techniques () used to support the required assertion and score an element
    /// 
    /// </summary>
    public class Survey
    {
        private SurveyStrategy surveyStrategy;

        public Survey(SurveyStrategy surveyStrategy)
        {
            this.surveyStrategy = surveyStrategy;
            QuestionStrategies = surveyStrategy.QuestionStrategies;
            //Questions questions = surveyStrategy.QuestionStrategies.
            //loadQuestionStack(questions);
       
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyManager"></param>
        /// <returns></returns>
        internal Question GetNextQuestion(SurveyManager surveyManager)
        {
            return surveyStrategy.GetNextQuestion(surveyManager);
        }

        /// <summary>
        /// 
        /// </summary>
        private QuestionStrategies QuestionStrategies { get; set; }

        private void loadQuestionStack(Questions questions)
        {
            SortedList<int, Question> orderedQuestions = new SortedList<int, Question>();
            Stack<Question> questionsStack = new Stack<Question>();

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


        public Question FirstQuestion { get; set; }
    }
}
