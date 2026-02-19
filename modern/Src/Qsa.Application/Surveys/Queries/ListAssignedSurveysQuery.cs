using Qsa.Application.Common.Interfaces;

namespace Qsa.Application.Surveys.Queries;

public sealed record ListAssignedSurveysQuery;

public sealed class ListAssignedSurveysQueryHandler(IUserContext userContext, ISurveyRepository surveyRepository)
{
    public async Task<IReadOnlyList<AssignedSurveyDto>> HandleAsync(ListAssignedSurveysQuery query, CancellationToken cancellationToken = default)
    {
        if (!userContext.IsAuthenticated || string.IsNullOrEmpty(userContext.UserId))
            throw new UnauthorizedAccessException("Not authenticated.");

        if (userContext.Role != "Surveyor")
            throw new UnauthorizedAccessException("Only Surveyors can list assigned surveys.");

        var results = await surveyRepository.ListAssignedSurveysAsync(userContext.UserId, cancellationToken);
        return results
            .OrderBy(x => x.Survey.DueDate)
            .ThenByDescending(x => x.Survey.Priority)
            .Select(x => new AssignedSurveyDto(
                x.Survey.Id.ToString(),
                x.Survey.Title,
                x.Survey.DueDate.ToString("yyyy-MM-dd"),
                x.Survey.Status.ToString(),
                x.Survey.Priority.ToString(),
                x.Survey.LocationName,
                x.AssignedAt.ToString("O")))
            .ToList();
    }
}
