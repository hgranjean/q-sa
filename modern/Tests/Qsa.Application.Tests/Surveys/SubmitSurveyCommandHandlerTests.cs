using Moq;
using Qsa.Application.Common.Interfaces;
using Qsa.Application.Surveys;
using Qsa.Application.Surveys.Commands;
using Qsa.Domain.Surveys;
using Xunit;

namespace Qsa.Application.Tests.Surveys;

public sealed class SubmitSurveyCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenRequiredItemUnanswered_ThrowsChecklistValidationExceptionWithMissingIds()
    {
        var userContext = new Mock<IUserContext>();
        userContext.Setup(x => x.IsAuthenticated).Returns(true);
        userContext.Setup(x => x.UserId).Returns("usr_svy_01");
        userContext.Setup(x => x.Role).Returns("Surveyor");

        var surveyId = Guid.NewGuid();
        var requiredItemId = Guid.NewGuid();
        var checklist = new SurveyChecklist
        {
            SurveyId = surveyId,
            Items =
            [
                new ChecklistItem { Id = requiredItemId, Text = "Required?", IsRequired = true, SortOrder = 1 }
            ]
        };

        var authorizer = new Mock<ISurveyAssignmentAuthorizer>();
        authorizer.Setup(x => x.EnsureUserAssignedAsync(surveyId, "usr_svy_01", It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var checklistProvider = new Mock<IChecklistProvider>();
        checklistProvider.Setup(x => x.GetChecklistAsync(surveyId, It.IsAny<CancellationToken>())).ReturnsAsync(checklist);

        var responseStore = new Mock<ISurveyResponseStore>();
        responseStore.Setup(x => x.GetResponsesAsync(surveyId, "usr_svy_01", It.IsAny<CancellationToken>())).ReturnsAsync(new List<SurveyResponse>());

        var lifecycle = new Mock<ISurveyLifecycle>();
        lifecycle.Setup(x => x.GetSurveyStatusAsync(surveyId, It.IsAny<CancellationToken>())).ReturnsAsync(SurveyStatus.InProgress);

        var handler = new SubmitSurveyCommandHandler(
            userContext.Object,
            authorizer.Object,
            checklistProvider.Object,
            responseStore.Object,
            lifecycle.Object);

        var ex = await Assert.ThrowsAsync<ChecklistValidationException>(() =>
            handler.HandleAsync(new SubmitSurveyCommand(surveyId), default));

        Assert.Single(ex.MissingRequiredItemIds);
        Assert.Equal(requiredItemId, ex.MissingRequiredItemIds[0]);
    }

    [Fact]
    public async Task HandleAsync_WhenAllRequiredAnswered_ReturnsSubmitResult()
    {
        var userContext = new Mock<IUserContext>();
        userContext.Setup(x => x.IsAuthenticated).Returns(true);
        userContext.Setup(x => x.UserId).Returns("usr_svy_01");
        userContext.Setup(x => x.Role).Returns("Surveyor");

        var surveyId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var checklist = new SurveyChecklist
        {
            SurveyId = surveyId,
            Items = [new ChecklistItem { Id = itemId, Text = "Required?", IsRequired = true, SortOrder = 1 }]
        };
        var responses = new List<SurveyResponse>
        {
            new() { SurveyId = surveyId, SurveyorUserId = "usr_svy_01", ItemId = itemId, Value = ChecklistResponseValue.Pass, UpdatedAt = DateTime.UtcNow }
        };

        var authorizer = new Mock<ISurveyAssignmentAuthorizer>();
        authorizer.Setup(x => x.EnsureUserAssignedAsync(surveyId, "usr_svy_01", It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var checklistProvider = new Mock<IChecklistProvider>();
        checklistProvider.Setup(x => x.GetChecklistAsync(surveyId, It.IsAny<CancellationToken>())).ReturnsAsync(checklist);

        var responseStore = new Mock<ISurveyResponseStore>();
        responseStore.Setup(x => x.GetResponsesAsync(surveyId, "usr_svy_01", It.IsAny<CancellationToken>())).ReturnsAsync(responses);

        var lifecycle = new Mock<ISurveyLifecycle>();
        lifecycle.Setup(x => x.GetSurveyStatusAsync(surveyId, It.IsAny<CancellationToken>())).ReturnsAsync(SurveyStatus.InProgress);
        lifecycle.Setup(x => x.MarkSubmittedAsync(surveyId, "usr_svy_01", It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var handler = new SubmitSurveyCommandHandler(
            userContext.Object,
            authorizer.Object,
            checklistProvider.Object,
            responseStore.Object,
            lifecycle.Object);

        var result = await handler.HandleAsync(new SubmitSurveyCommand(surveyId), default);

        Assert.NotNull(result);
        Assert.Equal(surveyId.ToString(), result.SurveyId);
        Assert.Equal("Submitted", result.Status);
    }
}
