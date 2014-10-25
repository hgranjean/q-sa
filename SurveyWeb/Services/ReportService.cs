using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Services
{
    public class ReportService
    {
        private readonly IPersistenceServices _persistenceService;
        private readonly SurveillanceService _surveillanceService;

        public ReportService(IPersistenceServices persistenceService, SurveillanceService surveillanceService)
        {
            _persistenceService = persistenceService;
            _surveillanceService = surveillanceService;
        }

        public ProgressReport GetProgressReport(string userId)
        {
            var algorithm = new ProgressReportAlgorithm(_persistenceService, _surveillanceService, userId);

            return algorithm.Execute();
        }
    }

    #region Algorithms

    internal abstract class ReportAlgorithm<T>
    {
        protected readonly IPersistenceServices _persistenceService;
        protected readonly SurveillanceService _surveillanceService;

        protected ReportAlgorithm(IPersistenceServices persistenceService, SurveillanceService surveillanceService)
        {
            _persistenceService = persistenceService;
            _surveillanceService = surveillanceService;
        }

        public abstract T Execute();
    }

    internal class ProgressReportAlgorithm : ReportAlgorithm<ProgressReport>
    {
        private readonly string _userId;

        public ProgressReportAlgorithm(IPersistenceServices persistenceService, SurveillanceService surveillanceService, string userId) :
            base(persistenceService, surveillanceService)
        {
            _userId = userId;
        }
        public override ProgressReport Execute()
        {
            var surveys = _persistenceService.GetSurveys();

            // Filtering out responses by user            
            var responses = _surveillanceService.GetResponses(_userId);

            var surveyModels = new List<TracerViewModel>();
            foreach (var response in responses)
            {
                var tracerModel = _persistenceService.LoadTracer(response);
                var survey = _persistenceService.GetSurvey(tracerModel.SurveyId);
                tracerModel.QuestionGroups = new QuestionGroupsViewModel(tracerModel.SurveyId, survey.QuestionGroups);
                surveyModels.Add(tracerModel);
            }

            // x - elements
            // y - areas
            // value - completion
            var areas = _surveillanceService.GetAreas(surveyModels.First().FacilityId.ToString()).ToList();
            var elements = surveyModels.Max(m => m.QuestionGroups.Count());
            decimal[,] elementsByAreas = new decimal[areas.Count(), elements];
            foreach (var survey in surveyModels)
            {
                try
                {
                    int qIndex = 0;
                    foreach (var questionGroup in survey.QuestionGroups)
                    {
                        int responseCount = 0;
                        foreach (var question in questionGroup.QuestionGroup.Questions)
                        {
                            if (survey.Responses[qIndex] != null)
                            {
                                responseCount++;
                            }
                            qIndex++;
                        }
                        var completionPercent = ((decimal)responseCount / (decimal)qIndex) * 100m;

                        elementsByAreas[areas.FindIndex(area => area.Id == survey.AreaId), questionGroup.Number] += completionPercent; // TODO aggregate
                    }
                }
                catch (Exception ex)
                {
                    // Skip problematic survey for now
                }
            }

            return new ProgressReport(elementsByAreas);
        }
    }

    #endregion
}