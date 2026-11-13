using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Identity.Abstractions.Authorization.Permissions;

public sealed class NotBlockedRequirement : IAuthorizationRequirement;

public sealed class NotBlockedAuthorizationHandler : AuthorizationHandler<NotBlockedRequirement>
{
    private readonly IEnumerable<IBlockedPrincipalCheck> _checks;

    public NotBlockedAuthorizationHandler(IEnumerable<IBlockedPrincipalCheck> checks)
    {
        ArgumentNullException.ThrowIfNull(checks);
        _checks = checks;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        NotBlockedRequirement requirement)
    {
        var cancellationToken = context.Resource is HttpContext httpContext
            ? httpContext.RequestAborted
            : CancellationToken.None;

        foreach (var check in _checks)
        {
            if (await check.IsBlockedAsync(context.User, cancellationToken))
            {
                context.Fail(new AuthorizationFailureReason(this, "The principal is blocked."));
                return;
            }
        }

        context.Succeed(requirement);
    }
}

public static class NotBlockedPolicyBuilderExtensions
{
    /// <summary>Adds the not-blocked requirement to a policy, such as a product's default policy.</summary>
    public static AuthorizationPolicyBuilder RequireNotBlocked(this AuthorizationPolicyBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder.AddRequirements(new NotBlockedRequirement());
    }
}
