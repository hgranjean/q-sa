using Qsa.Application.Common.Interfaces;
using Qsa.Domain.Surveys;

namespace Qsa.Application.Surveys.Commands;

public sealed record SubmitSurveyCommand(Guid SurveyId);

public sealed class SubmitSurveyCommandHandler(
    IUserContext userContext,
    ISurveyAssignmentAuthorizer authorizer,
    IChecklistProvider checklistProvider,
    ISurveyResponseStore responseStore,
    ISurveyLifecycle lifecycle)
{
    public async Task<SubmitResultDto> HandleAsync(SubmitSurveyCommand command, CancellationToken cancellationToken = default)
    {
        if (!userContext.IsAuthenticated || string.IsNullOrEmpty(userContext.UserId))
            throw new UnauthorizedAccessException("Not authenticated.");
        if (userContext.Role != "Surveyor")
            throw new UnauthorizedAccessException("Only Surveyors can submit surveys.");

        await authorizer.EnsureUserAssignedAsync(command.SurveyId, userContext.UserId, cancellationToken);

        var checklist = await checklistProvider.GetChecklistAsync(command.SurveyId, cancellationToken);
        if (checklist == null)
            throw new InvalidOperationException("Survey checklist not found.");

        var status = await lifecycle.GetSurveyStatusAsync(command.SurveyId, cancellationToken);
        if (status == SurveyStatus.Submitted || status == SurveyStatus.Completed)
            return new SubmitResultDto(command.SurveyId.ToString(), status.ToString(), DateTime.UtcNow.ToString("O"));

        var responses = await responseStore.GetResponsesAsync(command.SurveyId, userContext.UserId, cancellationToken);
        var responseByItem = responses.ToDictionary(r => r.ItemId);

        var requiredIds = checklist.Items.Where(i => i.IsRequired).Select(i => i.Id).ToList();
        var missing = requiredIds.Where(id => !responseByItem.ContainsKey(id)).ToList();
        if (missing.Count > 0)
            throw new ChecklistValidationException("All required items must be answered before submit.", missing);

        var submittedAt = DateTime.UtcNow;
        await lifecycle.MarkSubmittedAsync(command.SurveyId, userContext.UserId, submittedAt, cancellationToken);

        return new SubmitResultDto(command.SurveyId.ToString(), "Submitted", submittedAt.ToString("O"));
    }
}
