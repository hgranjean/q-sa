using Atum.Domain.Basis;
using Atum.Domain.Common;
using System;

namespace Atum.Domain.SurveyManagement
{
    [Serializable]
    public class SurveillanceSchedule : DomainObject
    {
        protected override void SetId(long id)
        {
            throw new NotImplementedException();
        }

        public Frequency Frequency { get; set; }

        public Survey Survey { get; set; }

        public Common.Environment Environment { get; set; }

        public string Title { get; set; }
    }
}
