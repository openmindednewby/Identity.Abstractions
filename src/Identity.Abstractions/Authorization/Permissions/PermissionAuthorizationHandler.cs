using Microsoft.AspNetCore.Authorization;

namespace Identity.Abstractions.Authorization.Permissions;

public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly PermissionMap _map;

    public PermissionAuthorizationHandler(PermissionMap map)
    {
        ArgumentNullException.ThrowIfNull(map);
        _map = map;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var roles = context.User.Identities
            .SelectMany(identity => identity.FindAll(identity.RoleClaimType))
            .Select(claim => claim.Value);

        if (_map.Grants(roles, requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
