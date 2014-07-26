using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Domain.SurveyManagement
{
    [Serializable]
    public class Score
    {
        public Score(int supporting, int opposing)
        {
            this.Supporting = supporting;
            this.Opposing = opposing;
        }


        public int Supporting { get; private set; }
        public int Opposing { get; private set; }
    }
}
