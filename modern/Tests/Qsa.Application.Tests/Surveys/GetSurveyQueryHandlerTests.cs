using Moq;
using Qsa.Application.Common.Interfaces;
using Qsa.Application.Surveys.Queries;
using Qsa.Domain.Surveys;
using Xunit;

namespace Qsa.Application.Tests.Surveys;

public sealed class GetSurveyQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenSurveyNotFound_ReturnsNull()
    {
        var userContext = new Mock<IUserContext>();
        userContext.Setup(x => x.IsAuthenticated).Returns(true);
        userContext.Setup(x => x.UserId).Returns("usr_svy_01");
        userContext.Setup(x => x.Role).Returns("Surveyor");

        var repo = new Mock<ISurveyRepository>();
        var surveyId = Guid.NewGuid();
        repo.Setup(x => x.GetSurveyByIdAsync(surveyId, It.IsAny<CancellationToken>())).ReturnsAsync((Survey?)null);

        var handler = new GetSurveyQueryHandler(userContext.Object, repo.Object);
        var result = await handler.HandleAsync(new GetSurveyQuery(surveyId), default);

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenSurveyNotAssignedToUser_ThrowsUnauthorizedAccessException()
    {
        var userContext = new Mock<IUserContext>();
        userContext.Setup(x => x.IsAuthenticated).Returns(true);
        userContext.Setup(x => x.UserId).Returns("usr_svy_01");
        userContext.Setup(x => x.Role).Returns("Surveyor");

        var surveyId = Guid.NewGuid();
        var survey = new Survey
        {
            Id = surveyId,
            Title = "Other Survey",
            DueDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = SurveyStatus.NotStarted,
            Priority = SurveyPriority.Medium
        };
        var repo = new Mock<ISurveyRepository>();
        repo.Setup(x => x.GetSurveyByIdAsync(surveyId, It.IsAny<CancellationToken>())).ReturnsAsync(survey);
        repo.Setup(x => x.IsSurveyAssignedToUserAsync(surveyId, "usr_svy_01", It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var handler = new GetSurveyQueryHandler(userContext.Object, repo.Object);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            handler.HandleAsync(new GetSurveyQuery(surveyId), default));
    }

    [Fact]
    public async Task HandleAsync_WhenSurveyAssignedToUser_ReturnsDetailDto()
    {
        var userContext = new Mock<IUserContext>();
        userContext.Setup(x => x.IsAuthenticated).Returns(true);
        userContext.Setup(x => x.UserId).Returns("usr_svy_01");
        userContext.Setup(x => x.Role).Returns("Surveyor");

        var surveyId = Guid.NewGuid();
        var assignedAt = DateTime.UtcNow.AddDays(-2);
        var survey = new Survey
        {
            Id = surveyId,
            Title = "My Survey",
            LocationName = "Chicago",
            DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)),
            Status = SurveyStatus.InProgress,
            Priority = SurveyPriority.High
        };
        var repo = new Mock<ISurveyRepository>();
        repo.Setup(x => x.GetSurveyByIdAsync(surveyId, It.IsAny<CancellationToken>())).ReturnsAsync(survey);
        repo.Setup(x => x.IsSurveyAssignedToUserAsync(surveyId, "usr_svy_01", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        repo.Setup(x => x.GetAssignedAtAsync(surveyId, "usr_svy_01", It.IsAny<CancellationToken>())).ReturnsAsync(assignedAt);

        var handler = new GetSurveyQueryHandler(userContext.Object, repo.Object);
        var result = await handler.HandleAsync(new GetSurveyQuery(surveyId), default);

        Assert.NotNull(result);
        Assert.Equal(surveyId.ToString(), result.Id);
        Assert.Equal("My Survey", result.Title);
        Assert.Equal("Chicago", result.LocationName);
        Assert.Equal("InProgress", result.Status);
        Assert.Equal("High", result.Priority);
    }
}
