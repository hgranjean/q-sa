using Qsa.Domain.Surveys;

namespace Qsa.Application.Common.Interfaces;

public interface ISurveyResponseStore
{
    Task<IReadOnlyList<SurveyResponse>> GetResponsesAsync(Guid surveyId, string userId, CancellationToken cancellationToken = default);
    Task UpsertResponseAsync(SurveyResponse response, CancellationToken cancellationToken = default);
}
