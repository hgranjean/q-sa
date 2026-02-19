using Qsa.Application.Common.Interfaces;

namespace Qsa.Infrastructure.Surveys;

public sealed class SurveyAssignmentAuthorizer(ISurveyRepository surveyRepository) : ISurveyAssignmentAuthorizer
{
    public async Task EnsureUserAssignedAsync(Guid surveyId, string userId, CancellationToken cancellationToken = default)
    {
        var assigned = await surveyRepository.IsSurveyAssignedToUserAsync(surveyId, userId, cancellationToken);
        if (!assigned)
            throw new UnauthorizedAccessException("Survey is not assigned to you.");
    }
}
