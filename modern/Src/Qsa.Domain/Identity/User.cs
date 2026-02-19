namespace Qsa.Domain.Identity;

/// <summary>Domain user identity (minimal for auth slice).</summary>
public class User
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required string DisplayName { get; init; }
    public required Role Role { get; init; }
}
