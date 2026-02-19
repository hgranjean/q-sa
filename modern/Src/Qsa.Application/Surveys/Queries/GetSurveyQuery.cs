using Qsa.Application.Common.Interfaces;

namespace Qsa.Application.Surveys.Queries;

public sealed record GetSurveyQuery(Guid SurveyId);

public sealed class GetSurveyQueryHandler(IUserContext userContext, ISurveyRepository surveyRepository)
{
    public async Task<SurveyDetailDto?> HandleAsync(GetSurveyQuery query, CancellationToken cancellationToken = default)
    {
        if (!userContext.IsAuthenticated || string.IsNullOrEmpty(userContext.UserId))
            throw new UnauthorizedAccessException("Not authenticated.");

        if (userContext.Role != "Surveyor")
            throw new UnauthorizedAccessException("Only Surveyors can view survey details.");

        var survey = await surveyRepository.GetSurveyByIdAsync(query.SurveyId, cancellationToken);
        if (survey == null)
            return null;

        var isAssigned = await surveyRepository.IsSurveyAssignedToUserAsync(query.SurveyId, userContext.UserId, cancellationToken);
        if (!isAssigned)
            throw new UnauthorizedAccessException("Survey is not assigned to you.");

        var assignedAt = await surveyRepository.GetAssignedAtAsync(query.SurveyId, userContext.UserId, cancellationToken);
        return new SurveyDetailDto(
            survey.Id.ToString(),
            survey.Title,
            survey.DueDate.ToString("yyyy-MM-dd"),
            survey.Status.ToString(),
            survey.Priority.ToString(),
            survey.LocationName,
            assignedAt?.ToString("O"));
    }
}
