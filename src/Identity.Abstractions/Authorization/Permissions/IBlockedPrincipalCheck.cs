using System.Security.Claims;

namespace Identity.Abstractions.Authorization.Permissions;

public interface IBlockedPrincipalCheck
{
    Task<bool> IsBlockedAsync(ClaimsPrincipal user, CancellationToken cancellationToken);
}
