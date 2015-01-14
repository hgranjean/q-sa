using Atum.Domain.Basis;
using Atum.Domain.Common;
using Atum.Domain.SurveyManagement;
using System;

namespace Atum.Domain.QualityManagement
{
    [Serializable]
    public class SurveillanceSchedule : DomainObject
    {

        public Frequency Frequency { get; set; }

        public Survey Survey { get; set; }

        public Common.Environment Environment { get; set; }

        public string Title { get; set; }
    }
}
