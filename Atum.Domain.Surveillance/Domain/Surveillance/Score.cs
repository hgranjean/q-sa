using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Domain.Surveillance
{
    [Serializable]
    public class Score
    {
        public Score(int supporting, int opposing)
        {
            // TODO: Complete member initialization
            this.Supporting = supporting;
            this.Opposing = opposing;
        }


        public int Supporting { get; private set; }
        public int Opposing { get; private set; }
    }
}
