using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Atum.Domain.Basis;


namespace Atum.Domain.Surveillance
{
    /// <summary>
    /// Survey - Survey is the set of techniques () used to support the required assertion and score an element
    /// 
    /// </summary>
    [Serializable]
    public class Survey : DomainObject// : IEnumerable<Question>
    {
        private readonly SurveyStrategy surveyStrategy;

        public Survey(SurveyStrategy surveyStrategy)
        {   
            this.surveyStrategy = surveyStrategy;
            QuestionStrategies = surveyStrategy.QuestionStrategies;
            //Questions questions = surveyStrategy.QuestionStrategies.
            //loadQuestionStack(questions);

        }

        public Survey()
        {
        }

        protected override void SetId(long id)
        {
            throw new NotImplementedException();
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

        private void LoadQuestionStack(Questions questions)
        {
            var orderedQuestions = new SortedList<int, Question>();
            var questionsStack = new Stack<Question>();

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

        public QuestionGroup AddQuestionGroup(string groupTitle)
        {
            QuestionGroup retVal = new QuestionGroup(groupTitle);
            if (this.QuestionGroups==null)
            {
                this.QuestionGroups = new QuestionGroups();
            }
            retVal.Number = QuestionGroups.Count + 1;
            QuestionGroups.Add(retVal.Number, retVal);
            return retVal;
        }


        public QuestionGroups QuestionGroups { get; set; }

        public IEnumerator<Question> GetEnumerator()
        {   
            return new QuestionEnumerator(null);
        }

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return new QuestionEnumerator(null);
        //}

        public class QuestionEnumerator : IEnumerator<Question>
        {
            private SurveyManager _manager;

            public void SetSurveyManager(SurveyManager surveyManager)
            {
                _manager = surveyManager;
                
                Current = _manager.CurrentQuestion;
            }

            public QuestionEnumerator(SurveyManager manager)
            {
                _manager = manager;

                if (manager != null)
                {
                    Current = manager.CurrentQuestion;
                }

            }
            public void Dispose()
            {
                _manager = null;
            }

            public bool MoveNextManager(Survey survey)
            {
                Current = survey.GetNextQuestion(_manager);

                return (Current != null);
            }

            public bool MoveNext()
            {
                Current = _manager.NextQuestion;
                
                return (Current != null);
            }

            public void Reset()
            {
            }

            public Question Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
}
