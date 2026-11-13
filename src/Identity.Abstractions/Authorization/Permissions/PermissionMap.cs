namespace Identity.Abstractions.Authorization.Permissions;

public sealed class PermissionMap
{
    private readonly List<string> _permissionOrder;
    private readonly HashSet<string> _permissions;
    private readonly List<string> _roleOrder;
    private readonly Dictionary<string, HashSet<string>> _byRole;

    public PermissionMap(
        IEnumerable<string> permissions,
        IReadOnlyDictionary<string, IReadOnlyCollection<string>> rolePermissions)
    {
        ArgumentNullException.ThrowIfNull(permissions);
        ArgumentNullException.ThrowIfNull(rolePermissions);

        _permissionOrder = permissions.Distinct(StringComparer.Ordinal).ToList();
        _permissions = new HashSet<string>(_permissionOrder, StringComparer.Ordinal);
        _roleOrder = rolePermissions.Keys.ToList();
        _byRole = rolePermissions.ToDictionary(
            pair => pair.Key,
            pair => new HashSet<string>(pair.Value, StringComparer.Ordinal),
            StringComparer.Ordinal);

        if (_permissionOrder.Count == 0)
        {
            throw new ArgumentException("At least one permission must be declared.", nameof(permissions));
        }

        if (_permissionOrder.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("A permission name cannot be blank.", nameof(permissions));
        }

        var undeclared = _byRole.Values
            .SelectMany(granted => granted)
            .Where(permission => !_permissions.Contains(permission))
            .Distinct(StringComparer.Ordinal)
            .ToList();

        if (undeclared.Count > 0)
        {
            throw new ArgumentException(
                $"Roles map permissions that are not declared: {string.Join(", ", undeclared)}.",
                nameof(rolePermissions));
        }
    }

    public IReadOnlyList<string> Permissions => _permissionOrder;

    public IReadOnlyList<string> Roles => _roleOrder;

    public bool IsDeclared(string? permission) => permission is not null && _permissions.Contains(permission);

    /// <summary>Every permission granted by at least one of the given roles; unknown roles grant nothing.</summary>
    public IReadOnlySet<string> PermissionsFor(IEnumerable<string> roles)
    {
        ArgumentNullException.ThrowIfNull(roles);

        var granted = new HashSet<string>(StringComparer.Ordinal);
        foreach (var role in roles)
        {
            if (_byRole.TryGetValue(role, out var permissions))
            {
                granted.UnionWith(permissions);
            }
        }

        return granted;
    }

    public bool Grants(IEnumerable<string> roles, string permission)
    {
        ArgumentNullException.ThrowIfNull(roles);
        ArgumentNullException.ThrowIfNull(permission);

        return roles.Any(role => _byRole.TryGetValue(role, out var granted) && granted.Contains(permission));
    }

    /// <summary>The role x permission matrix in declaration order, for a read-only UI.</summary>
    public PermissionMatrix ToMatrix() => new(
        _roleOrder,
        _permissionOrder,
        _roleOrder
            .Select(role => new RolePermissions(
                role,
                _permissionOrder.Where(permission => _byRole[role].Contains(permission)).ToList()))
            .ToList());
}
