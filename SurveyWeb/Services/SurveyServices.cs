using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Atum.Domain.Surveillance;

namespace MvcApplication1
{
    internal class SurveyServices
    {
        private static SurveyManager _surverManager;

        static SurveyServices()
        {   
        }

        public static SurveyManager GetSurveyManager(Survey survey)
        {
            _surverManager = new SurveyManager(survey);
            return _surverManager;
        }
    }
}