using Qsa.Domain.Surveys;

namespace Qsa.Application.Common.Interfaces;

public interface ISurveyLifecycle
{
    Task<SurveyStatus> GetSurveyStatusAsync(Guid surveyId, CancellationToken cancellationToken = default);
    Task MarkSubmittedAsync(Guid surveyId, string userId, DateTime submittedAt, CancellationToken cancellationToken = default);
}
