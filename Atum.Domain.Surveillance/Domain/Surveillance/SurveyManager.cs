using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Domain.Surveillance
{
    public class SurveyManager
    {
        private Survey survey;
//        private QuestionStrategies questionStrategies;

        public SurveyManager(Survey survey)
        {
            // TODO: Complete member initialization
            this.survey = survey;

            //Initialize Manager
            Init();
        }

        private void Init()
        {
            //Set Next Questions
            setNextQuestions();
            CompletedQuestions = new Questions();
            this.Responses = new Responses();
        }

        private void setNextQuestions()
        {
            //The set of next questions depend on the state of the survey
            //Does the current state require a new QuestionStrategy Set
            //i.e. we have achieved maximum confidence for the element under Surveillance

           //Depends on current state of
           NextQuestion = this.survey.GetNextQuestion(this);
           
            CurrentQuestion = NextQuestion;
        }



        /// <summary>
        /// AcceptResponse
        /// </summary>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public void AcceptResponse(Question question, ResponseChoice answer)
        {
            ////Create and add response to response collection
            //Response response = new Response(question, answer);
            //this.Responses.Add(response);
            //this.CompletedQuestions.Add(question);


            //if (PreviousAnswer != null)
            //{
            //    //Calculate/Set Current Score
            //    if (answer.Supports(PreviousAnswer))
            //    {
            //        CurrentScore += answer.PositiveScore;
            //    }
            //    else if (answer.Opposes(PreviousAnswer))
            //    {
            //        CurrentScore += answer.NegativeScore;
            //    }
            //}
            //else
            //{
            //    CurrentScore = answer.PositiveScore;
            //}

            //this.PreviousAnswer = answer;

            ////Find/Select/Set NextQuestion
            //setNextQuestions();

        }

        public Question CurrentQuestion { get; set; }

        public Responses Responses { get; private set; }

        public ResponseChoice PreviousAnswer { get; set; }

        public Questions NextQuestions { get; set; }

        public bool SurveyComplete { get; set; }

        public int CurrentScore { get; set; }

        public Questions CompletedQuestions { get; set; }

        public Question NextQuestion { get; set; }
    }
}
