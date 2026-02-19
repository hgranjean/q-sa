using Moq;
using Qsa.Application.Common.Interfaces;
using Qsa.Application.Surveys.Queries;
using Qsa.Domain.Surveys;
using Xunit;

namespace Qsa.Application.Tests.Surveys;

public sealed class ListAssignedSurveysQueryHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenSurveyor_ReturnsOnlyTheirAssignedSurveys()
    {
        var userContext = new Mock<IUserContext>();
        userContext.Setup(x => x.IsAuthenticated).Returns(true);
        userContext.Setup(x => x.UserId).Returns("usr_svy_01");
        userContext.Setup(x => x.Role).Returns("Surveyor");

        var survey1 = new Survey
        {
            Id = Guid.NewGuid(),
            Title = "Survey A",
            DueDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = SurveyStatus.NotStarted,
            Priority = SurveyPriority.High
        };
        var repo = new Mock<ISurveyRepository>();
        repo.Setup(x => x.ListAssignedSurveysAsync("usr_svy_01", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<(Survey Survey, DateTime AssignedAt)> { (survey1, DateTime.UtcNow.AddDays(-1)) });

        var handler = new ListAssignedSurveysQueryHandler(userContext.Object, repo.Object);
        var result = await handler.HandleAsync(new ListAssignedSurveysQuery(), default);

        Assert.Single(result);
        Assert.Equal(survey1.Id.ToString(), result[0].Id);
        Assert.Equal("Survey A", result[0].Title);
    }

    [Fact]
    public async Task HandleAsync_WhenNotSurveyor_ThrowsUnauthorizedAccessException()
    {
        var userContext = new Mock<IUserContext>();
        userContext.Setup(x => x.IsAuthenticated).Returns(true);
        userContext.Setup(x => x.UserId).Returns("usr_mgr_01");
        userContext.Setup(x => x.Role).Returns("Manager");

        var repo = new Mock<ISurveyRepository>();
        var handler = new ListAssignedSurveysQueryHandler(userContext.Object, repo.Object);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            handler.HandleAsync(new ListAssignedSurveysQuery(), default));
    }
}
