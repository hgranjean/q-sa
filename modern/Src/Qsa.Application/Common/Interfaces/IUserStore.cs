using Qsa.Domain.Identity;

namespace Qsa.Application.Common.Interfaces;

/// <summary>User store abstraction (implemented in Infrastructure).</summary>
public interface IUserStore
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    IReadOnlyList<User> GetSeededUsers();
}
