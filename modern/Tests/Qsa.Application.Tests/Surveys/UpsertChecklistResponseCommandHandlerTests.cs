using Moq;
using Qsa.Application.Common.Interfaces;
using Qsa.Application.Surveys.Commands;
using Qsa.Domain.Surveys;
using Xunit;

namespace Qsa.Application.Tests.Surveys;

public sealed class UpsertChecklistResponseCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenNotAssigned_ThrowsUnauthorizedAccessException()
    {
        var userContext = new Mock<IUserContext>();
        userContext.Setup(x => x.IsAuthenticated).Returns(true);
        userContext.Setup(x => x.UserId).Returns("usr_svy_01");
        userContext.Setup(x => x.Role).Returns("Surveyor");

        var surveyId = Guid.NewGuid();
        var itemId = Guid.NewGuid();
        var authorizer = new Mock<ISurveyAssignmentAuthorizer>();
        authorizer.Setup(x => x.EnsureUserAssignedAsync(surveyId, "usr_svy_01", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new UnauthorizedAccessException("Survey is not assigned to you."));

        var handler = new UpsertChecklistResponseCommandHandler(
            userContext.Object,
            authorizer.Object,
            Mock.Of<IChecklistProvider>(),
            Mock.Of<ISurveyResponseStore>(),
            Mock.Of<ISurveyLifecycle>());

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            handler.HandleAsync(new UpsertChecklistResponseCommand(surveyId, itemId, "Pass", null), default));
    }

    [Fact]
    public async Task HandleAsync_WhenAssigned_UpsertsAndReturnsResponseSavedDto()
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
            Items = [new ChecklistItem { Id = itemId, Text = "Item?", IsRequired = false, SortOrder = 1 }]
        };

        var authorizer = new Mock<ISurveyAssignmentAuthorizer>();
        authorizer.Setup(x => x.EnsureUserAssignedAsync(surveyId, "usr_svy_01", It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var checklistProvider = new Mock<IChecklistProvider>();
        checklistProvider.Setup(x => x.GetChecklistAsync(surveyId, It.IsAny<CancellationToken>())).ReturnsAsync(checklist);

        var responseStore = new Mock<ISurveyResponseStore>();
        responseStore.Setup(x => x.UpsertResponseAsync(It.IsAny<SurveyResponse>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var lifecycle = new Mock<ISurveyLifecycle>();
        lifecycle.Setup(x => x.GetSurveyStatusAsync(surveyId, It.IsAny<CancellationToken>())).ReturnsAsync(SurveyStatus.InProgress);

        var handler = new UpsertChecklistResponseCommandHandler(
            userContext.Object,
            authorizer.Object,
            checklistProvider.Object,
            responseStore.Object,
            lifecycle.Object);

        var result = await handler.HandleAsync(new UpsertChecklistResponseCommand(surveyId, itemId, "Fail", "Note"), default);

        Assert.Equal(itemId.ToString(), result.ItemId);
        responseStore.Verify(x => x.UpsertResponseAsync(It.Is<SurveyResponse>(r =>
            r.SurveyId == surveyId && r.SurveyorUserId == "usr_svy_01" && r.ItemId == itemId &&
            r.Value == ChecklistResponseValue.Fail && r.Notes == "Note"), It.IsAny<CancellationToken>()), Times.Once);
    }
}
