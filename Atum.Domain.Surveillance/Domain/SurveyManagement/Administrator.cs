
using System;
using Atum.Domain.Common;
namespace Atum.Domain.SurveyManagement
{
    [Serializable]
    public class Administrator : Person
    {
        public Survey MySurvey { get; set; }
    }

}
