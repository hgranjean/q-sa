using Atum.Domain.Common;

using System;
namespace Atum.Domain.Surveillance
{
    [Serializable]
    public class SurveyBasis : Document
    {

        public TableOfContents TableOfContents { get; set; }

    }
}