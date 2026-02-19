using Moq;
using Qsa.Application.Auth;
using Qsa.Application.Common.Interfaces;
using Qsa.Domain.Identity;
using Xunit;

namespace Qsa.Application.Tests.Auth;

public sealed class AuthenticateDevUserCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_WhenUserExists_ReturnsAuthResultWithTokenAndUser()
    {
        var user = new User
        {
            Id = "usr_1",
            Email = "vp@example.com",
            DisplayName = "VP User",
            Role = Role.VP
        };
        var userStore = new Mock<IUserStore>();
        userStore.Setup(x => x.GetByEmailAsync("vp@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        var tokenService = new Mock<ITokenService>();
        tokenService.Setup(x => x.IssueToken(It.IsAny<User>())).Returns("jwt-token");

        var handler = new AuthenticateDevUserCommandHandler(userStore.Object, tokenService.Object);
        var result = await handler.HandleAsync(new AuthenticateDevUserCommand("vp@example.com"), default);

        Assert.NotNull(result);
        Assert.Equal("jwt-token", result.Token);
        Assert.Equal("usr_1", result.User.Id);
        Assert.Equal("vp@example.com", result.User.Email);
        Assert.Equal("VP User", result.User.DisplayName);
        Assert.Equal("VP", result.User.Role);
    }

    [Fact]
    public async Task HandleAsync_WhenUserNotFound_ReturnsNull()
    {
        var userStore = new Mock<IUserStore>();
        userStore.Setup(x => x.GetByEmailAsync("unknown@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
        var tokenService = new Mock<ITokenService>();

        var handler = new AuthenticateDevUserCommandHandler(userStore.Object, tokenService.Object);
        var result = await handler.HandleAsync(new AuthenticateDevUserCommand("unknown@example.com"), default);

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenRoleProvided_OverridesUserRole()
    {
        var user = new User
        {
            Id = "usr_1",
            Email = "manager@example.com",
            DisplayName = "Manager User",
            Role = Role.Manager
        };
        var userStore = new Mock<IUserStore>();
        userStore.Setup(x => x.GetByEmailAsync("manager@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        var tokenService = new Mock<ITokenService>();
        tokenService.Setup(x => x.IssueToken(It.Is<User>(u => u.Role == Role.Surveyor))).Returns("jwt-token");

        var handler = new AuthenticateDevUserCommandHandler(userStore.Object, tokenService.Object);
        var result = await handler.HandleAsync(new AuthenticateDevUserCommand("manager@example.com", "Surveyor"), default);

        Assert.NotNull(result);
        Assert.Equal("Surveyor", result.User.Role);
    }
}
