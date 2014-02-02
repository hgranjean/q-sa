
using System;
using Atum.Domain.Common;
namespace Atum.Domain.Surveillance
{
    [Serializable]
    public class Administrator : Person
    {
        public Survey MySurvey { get; set; }
    }

}
