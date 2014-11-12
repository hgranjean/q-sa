using Atum.Domain.Basis;
using System;
using System.Collections.Generic;

namespace Atum.Domain.SurveyManagement
{
    [Serializable]
    public class ResponseChoice : DomainObject
    {
        public static string CompliantString = "Compliant";
        public static string NAString = "N/A";
        public static string NotScoredString = "Not Scored";
        public static string FollowUpCompletedString = "Follow-Up Completed";
        public static string NonCompliantString = "Non Compliant";

        protected ResponseChoice()
        {
        }

        public ResponseChoice(string choiceText)
        {
            this.Text = choiceText;
        }
        public string Text { get; set; }

        public string Value { get; set; }

        public string Number { get; set; }

        public string Key { get; set; }


        public void SetIdInternal(long id)
        {
            Id = id;
        }
    }
}
