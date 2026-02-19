using Qsa.Domain.Identity;

namespace Qsa.Application.Common.Interfaces;

/// <summary>Token issuance/validation (implemented in Infrastructure).</summary>
public interface ITokenService
{
    string IssueToken(User user);
    bool ValidateToken(string token, out string? userId);
}
