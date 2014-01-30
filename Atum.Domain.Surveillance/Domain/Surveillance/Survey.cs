using System;
using System.Collections.Generic;
using System.Text;


namespace Atum.Domain.Surveillance
{
    /// <summary>
    /// Survey - Survey is the set of techniques () used to support the required assertion and score an element
    /// 
    /// </summary>
    [Serializable]
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

        public Survey()
        {
            // TODO: Complete member initialization
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

        public SurveyType SurveyType { get; set; }

        public QuestionGroup AddQuestionGroup()
        {

            return AddQuestionGroup(string.Empty);
        }

        public QuestionGroup AddQuestionGroup(string groupTittle)
        {
            QuestionGroup retVal = new QuestionGroup(groupTittle);
            if (this.QuestionGroups == null)
            {
                this.QuestionGroups = new QuestionGroups();
            }
            retVal.Number = QuestionGroups.Count + 1;
            QuestionGroups.Add(retVal.Number, retVal);
            return retVal;
        }


        public QuestionGroups QuestionGroups { get; set; }
    }
}
