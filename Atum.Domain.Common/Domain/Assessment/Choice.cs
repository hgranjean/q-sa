using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Domain.Assessment
{
    public class AnswerChoice
    {
        private Score score;

        public AnswerChoice(string response,Score score, string[] supportedQuestions, string[] opposedQuestions)
        {
            // TODO: Complete member initialization
            this.score = score;
            //this.followSpec = followSpec;
            this.Response = response;
            this.NegativeScore = score.Opposing;
            this.PositiveScore = score.Supporting;

            Init(supportedQuestions, opposedQuestions);
        }

        private void Init(string[] supportedQuestions, string[] opposedQuestions)
        {
            this.SupportedQuestions = new List<string>();
            SupportedQuestions.AddRange(supportedQuestions);
            this.OpposedQuestions = new List<string>();
            OpposedQuestions.AddRange(supportedQuestions);
        }
        

        public int PositiveScore { get; set; }

        public int NegativeScore { get; set; }

        public string Response { get; set; }


        public bool Supports(AnswerChoice answer)
        {
            return this.SupportedQuestions.Contains(answer.Response);
        }

        /// <summary>
        /// Checks to see if an answer supports a previous answer 
        /// so this should take a string or an AnswerChoice
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        internal bool Opposes(AnswerChoice answer)
        {
            return this.OpposedQuestions.Contains(answer.Response);
        }

        public List<string> SupportedQuestions { get; set; }

        public List<string> OpposedQuestions { get; set; }
    }
}
