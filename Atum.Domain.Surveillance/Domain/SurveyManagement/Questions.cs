using System;
using System.Collections.Generic;

namespace Atum.Domain.SurveyManagement
{
    [Serializable]
    public class Questions : List<Question>
    {
        //public Question PreviousQuestion { get; set; }

        public Questions()
        {   
        }
    }
}
