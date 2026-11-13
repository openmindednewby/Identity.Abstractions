using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Identity.Abstractions.Authorization.Permissions;

public sealed class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallback;
    private readonly PermissionMap _map;

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options, PermissionMap map)
    {
        ArgumentNullException.ThrowIfNull(map);
        _fallback = new DefaultAuthorizationPolicyProvider(options);
        _map = map;
    }

    /// <summary>A perm:name policy (signed in, not blocked, permission granted) or the default provider's policy.</summary>
    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!PermissionPolicy.TryGetPermission(policyName, out var permission))
        {
            return _fallback.GetPolicyAsync(policyName);
        }

        if (!_map.IsDeclared(permission))
        {
            return Task.FromResult<AuthorizationPolicy?>(null);
        }

        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireNotBlocked()
            .AddRequirements(new PermissionRequirement(permission))
            .Build();

        return Task.FromResult<AuthorizationPolicy?>(policy);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallback.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _fallback.GetFallbackPolicyAsync();
}
