using Qsa.Application.Common.Interfaces;

namespace Qsa.Application.Auth;

/// <summary>Query for the current authenticated user.</summary>
public sealed record GetCurrentUserQuery;

/// <summary>Handler for GetCurrentUserQuery.</summary>
public sealed class GetCurrentUserQueryHandler(IUserContext userContext, IUserStore userStore)
{
    public async Task<UserDto?> HandleAsync(GetCurrentUserQuery query, CancellationToken cancellationToken = default)
    {
        if (!userContext.IsAuthenticated || string.IsNullOrEmpty(userContext.UserId))
            return null;

        var email = userContext.Email;
        if (string.IsNullOrEmpty(email))
            return null;

        var user = await userStore.GetByEmailAsync(email, cancellationToken);
        if (user == null)
            return null;

        return new UserDto(
            user.Id,
            user.Email,
            user.DisplayName,
            user.Role.ToString());
    }
}
