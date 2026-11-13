using System.Diagnostics.CodeAnalysis;

namespace Identity.Abstractions.Authorization.Permissions;

public static class PermissionPolicy
{
    public const string Prefix = "perm:";

    /// <summary>The policy name an endpoint names to require the given permission.</summary>
    public static string For(string permission)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(permission);
        return Prefix + permission;
    }

    public static bool TryGetPermission(string? policyName, [NotNullWhen(true)] out string? permission)
    {
        var isPermissionPolicy = policyName is not null
            && policyName.StartsWith(Prefix, StringComparison.Ordinal)
            && policyName.Length > Prefix.Length;

        permission = isPermissionPolicy ? policyName![Prefix.Length..] : null;
        return isPermissionPolicy;
    }
}
