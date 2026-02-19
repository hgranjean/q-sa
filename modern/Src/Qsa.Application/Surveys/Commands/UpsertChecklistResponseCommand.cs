using Qsa.Application.Common.Interfaces;
using Qsa.Domain.Surveys;

namespace Qsa.Application.Surveys.Commands;

public sealed record UpsertChecklistResponseCommand(Guid SurveyId, Guid ItemId, string Value, string? Notes);

public sealed class UpsertChecklistResponseCommandHandler(
    IUserContext userContext,
    ISurveyAssignmentAuthorizer authorizer,
    IChecklistProvider checklistProvider,
    ISurveyResponseStore responseStore,
    ISurveyLifecycle lifecycle)
{
    public async Task<ResponseSavedDto> HandleAsync(UpsertChecklistResponseCommand command, CancellationToken cancellationToken = default)
    {
        if (!userContext.IsAuthenticated || string.IsNullOrEmpty(userContext.UserId))
            throw new UnauthorizedAccessException("Not authenticated.");
        if (userContext.Role != "Surveyor")
            throw new UnauthorizedAccessException("Only Surveyors can save responses.");

        await authorizer.EnsureUserAssignedAsync(command.SurveyId, userContext.UserId, cancellationToken);

        var status = await lifecycle.GetSurveyStatusAsync(command.SurveyId, cancellationToken);
        if (status == SurveyStatus.Submitted || status == SurveyStatus.Completed)
            throw new InvalidOperationException("Survey is already submitted; cannot update responses.");

        if (!Enum.TryParse<ChecklistResponseValue>(command.Value, true, out var value))
            throw new ArgumentException("Value must be Pass, Fail, or NA.", nameof(command));

        var checklist = await checklistProvider.GetChecklistAsync(command.SurveyId, cancellationToken);
        if (checklist?.Items.All(i => i.Id != command.ItemId) ?? true)
            throw new ArgumentException("Item not found in checklist.", nameof(command));

        var updatedAt = DateTime.UtcNow;
        var response = new SurveyResponse
        {
            SurveyId = command.SurveyId,
            SurveyorUserId = userContext.UserId,
            ItemId = command.ItemId,
            Value = value,
            Notes = command.Notes,
            UpdatedAt = updatedAt
        };
        await responseStore.UpsertResponseAsync(response, cancellationToken);

        return new ResponseSavedDto(command.ItemId.ToString(), updatedAt.ToString("O"));
    }
}
