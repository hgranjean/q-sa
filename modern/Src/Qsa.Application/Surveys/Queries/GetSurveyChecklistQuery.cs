using Qsa.Application.Common.Interfaces;

namespace Qsa.Application.Surveys.Queries;

public sealed record GetSurveyChecklistQuery(Guid SurveyId);

public sealed class GetSurveyChecklistQueryHandler(
    IUserContext userContext,
    ISurveyAssignmentAuthorizer authorizer,
    IChecklistProvider checklistProvider,
    ISurveyResponseStore responseStore,
    ISurveyLifecycle lifecycle)
{
    public async Task<SurveyChecklistDto?> HandleAsync(GetSurveyChecklistQuery query, CancellationToken cancellationToken = default)
    {
        if (!userContext.IsAuthenticated || string.IsNullOrEmpty(userContext.UserId))
            throw new UnauthorizedAccessException("Not authenticated.");
        if (userContext.Role != "Surveyor")
            throw new UnauthorizedAccessException("Only Surveyors can view checklists.");

        await authorizer.EnsureUserAssignedAsync(query.SurveyId, userContext.UserId, cancellationToken);

        var checklist = await checklistProvider.GetChecklistAsync(query.SurveyId, cancellationToken);
        if (checklist == null)
            return null;

        var status = await lifecycle.GetSurveyStatusAsync(query.SurveyId, cancellationToken);
        var responses = await responseStore.GetResponsesAsync(query.SurveyId, userContext.UserId, cancellationToken);

        var items = checklist.Items
            .OrderBy(i => i.SortOrder)
            .Select(i => new ChecklistItemDto(i.Id.ToString(), i.Text, i.IsRequired, i.SortOrder))
            .ToArray();
        var responseDtos = responses
            .Select(r => new ChecklistResponseDto(r.ItemId.ToString(), r.Value.ToString(), r.Notes, r.UpdatedAt.ToString("O")))
            .ToArray();

        return new SurveyChecklistDto(
            query.SurveyId.ToString(),
            status.ToString(),
            items,
            responseDtos);
    }
}
