using System;
namespace Atum.Domain.Surveillance
{
    using System.Collections;

    [Serializable]
    public class SurveyResponse
    {
        // Associations

        public Responses Responses { get; set; }

        public Survey Survey { get; set; }

        public Respondent Respondent { get; set; }

    }
}
