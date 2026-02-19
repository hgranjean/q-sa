namespace Qsa.Application.Common.Interfaces;

/// <summary>Current user context (implemented in Infrastructure from HTTP claims).</summary>
public interface IUserContext
{
    string? UserId { get; }
    string? Email { get; }
    string? Role { get; }
    bool IsAuthenticated { get; }
}
