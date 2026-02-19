using Qsa.Application.Common.Interfaces;
using Qsa.Domain.Surveys;

namespace Qsa.Infrastructure.Surveys;

public sealed class InMemorySurveyLifecycle(ISurveyRepository surveyRepository) : ISurveyLifecycle
{
    public async Task<SurveyStatus> GetSurveyStatusAsync(Guid surveyId, CancellationToken cancellationToken = default)
    {
        var survey = await surveyRepository.GetSurveyByIdAsync(surveyId, cancellationToken);
        return survey?.Status ?? SurveyStatus.NotStarted;
    }

    public Task MarkSubmittedAsync(Guid surveyId, string userId, DateTime submittedAt, CancellationToken cancellationToken = default)
    {
        return surveyRepository.SetSurveyStatusAsync(surveyId, SurveyStatus.Submitted, cancellationToken);
    }
}
