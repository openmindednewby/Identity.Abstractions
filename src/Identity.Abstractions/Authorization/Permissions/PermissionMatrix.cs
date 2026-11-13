namespace Identity.Abstractions.Authorization.Permissions;

public sealed record PermissionMatrix(
    IReadOnlyList<string> Roles,
    IReadOnlyList<string> Permissions,
    IReadOnlyList<RolePermissions> Rows);

public sealed record RolePermissions(string Role, IReadOnlyList<string> Permissions);
