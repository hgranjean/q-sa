using Qsa.Application.Common.Interfaces;

namespace Qsa.Application.Auth;

/// <summary>Command to authenticate a dev user (stub mode).</summary>
public sealed record AuthenticateDevUserCommand(string Email, string? Role = null);

/// <summary>Handler for AuthenticateDevUserCommand.</summary>
public sealed class AuthenticateDevUserCommandHandler(IUserStore userStore, ITokenService tokenService)
{
    public async Task<AuthResult?> HandleAsync(AuthenticateDevUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userStore.GetByEmailAsync(command.Email, cancellationToken);
        if (user == null)
            return null;

        var role = command.Role != null && Enum.TryParse<Qsa.Domain.Identity.Role>(command.Role, true, out var r)
            ? r
            : user.Role;

        var effectiveUser = new Qsa.Domain.Identity.User
        {
            Id = user.Id,
            Email = user.Email,
            DisplayName = user.DisplayName,
            Role = role
        };

        var token = tokenService.IssueToken(effectiveUser);
        var dto = new UserDto(
            effectiveUser.Id,
            effectiveUser.Email,
            effectiveUser.DisplayName,
            effectiveUser.Role.ToString());

        return new AuthResult(token, dto);
    }
}
