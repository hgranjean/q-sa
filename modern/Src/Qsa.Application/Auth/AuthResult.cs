namespace Qsa.Application.Auth;

/// <summary>Result of dev authentication.</summary>
public sealed record AuthResult(string Token, UserDto User);

/// <summary>User DTO for API responses.</summary>
public sealed record UserDto(string Id, string Email, string DisplayName, string Role);
