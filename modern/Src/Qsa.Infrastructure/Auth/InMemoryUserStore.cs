using Microsoft.Extensions.Options;
using Qsa.Application.Common.Interfaces;
using Qsa.Domain.Identity;

namespace Qsa.Infrastructure.Auth;

public sealed class InMemoryUserStore : IUserStore
{
    private readonly List<User> _users;
    private readonly bool _useDevAuth;

    public InMemoryUserStore(IOptions<DevAuthOptions> options)
    {
        _useDevAuth = options.Value.Enabled;
        _users =
        [
            new User { Id = "usr_vp_01", Email = "vp@example.com", DisplayName = "VP User", Role = Role.VP },
            new User { Id = "usr_mgr_01", Email = "manager@example.com", DisplayName = "Manager User", Role = Role.Manager },
            new User { Id = "usr_svy_01", Email = "surveyor@example.com", DisplayName = "Surveyor User", Role = Role.Surveyor },
            new User { Id = "usr_svy_02", Email = "surveyor2@example.com", DisplayName = "Surveyor Two", Role = Role.Surveyor }
        ];
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        if (!_useDevAuth)
            return Task.FromResult<User?>(null);

        var user = _users.Find(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(user);
    }

    public IReadOnlyList<User> GetSeededUsers() => _users;
}

public sealed class DevAuthOptions
{
    public const string SectionName = "Auth";
    public bool UseDevAuth { get; set; }
    public bool Enabled => UseDevAuth;
}
