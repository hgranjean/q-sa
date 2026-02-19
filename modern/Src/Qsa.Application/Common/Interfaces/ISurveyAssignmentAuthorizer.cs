namespace Qsa.Application.Common.Interfaces;

public interface ISurveyAssignmentAuthorizer
{
    /// <summary>Throws if survey is not assigned to the user.</summary>
    Task EnsureUserAssignedAsync(Guid surveyId, string userId, CancellationToken cancellationToken = default);
}
