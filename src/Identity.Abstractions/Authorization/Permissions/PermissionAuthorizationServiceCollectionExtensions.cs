using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Identity.Abstractions.Authorization.Permissions;

public static class PermissionAuthorizationServiceCollectionExtensions
{
    /// <summary>Registers the permission map, the perm:name policy provider and both authorization handlers.</summary>
    public static IServiceCollection AddPermissionAuthorization(this IServiceCollection services, PermissionMap map)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(map);

        services.AddAuthorizationCore();
        services.TryAddSingleton(map);
        services.Replace(ServiceDescriptor.Singleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>());
        services.TryAddEnumerable(ServiceDescriptor.Scoped<IAuthorizationHandler, PermissionAuthorizationHandler>());
        services.TryAddEnumerable(ServiceDescriptor.Scoped<IAuthorizationHandler, NotBlockedAuthorizationHandler>());
        return services;
    }
}
