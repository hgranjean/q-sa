using Qsa.Domain.Surveys;

namespace Qsa.Application.Common.Interfaces;

public interface ISurveyRepository
{
    Task<IReadOnlyList<(Survey Survey, DateTime AssignedAt)>> ListAssignedSurveysAsync(string surveyorUserId, CancellationToken cancellationToken = default);
    Task<Survey?> GetSurveyByIdAsync(Guid surveyId, CancellationToken cancellationToken = default);
    Task<bool> IsSurveyAssignedToUserAsync(Guid surveyId, string userId, CancellationToken cancellationToken = default);
Task<DateTime?> GetAssignedAtAsync(Guid surveyId, string userId, CancellationToken cancellationToken = default);
    Task SetSurveyStatusAsync(Guid surveyId, SurveyStatus status, CancellationToken cancellationToken = default);
}
