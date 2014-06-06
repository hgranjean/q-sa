using Atum.Domain.Common;

using System;
namespace Atum.Domain.SurveyManagement
{
    [Serializable]
    public class SurveyBasis : Document
    {

        public TableOfContents TableOfContents { get; set; }

    }
}