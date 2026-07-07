namespace Identity.Abstractions.Authorization;

/// <summary>
/// A product's convention for the realm roles it manages for a tenant: the full
/// <see cref="Managed"/> set — the only roles the product's identity-provider
/// admin client adds or removes — and the <see cref="Assignable"/> subset a
/// tenant owner may grant to a user (excludes elevated roles such as a
/// platform-admin or the tenant-owner role itself).
/// </summary>
/// <remarks>
/// <para>
/// Product-agnostic: each product (Kefi, Erevna, Katalogos, ...) supplies its own
/// role <em>strings</em>; this type is the reusable "managed set + assignable
/// subset + guard" convention behind them, so the multi-role admin logic no longer
/// has to be re-implemented per product. Guards compare with
/// <see cref="StringComparer.Ordinal"/>.
/// </para>
/// <para>
/// Immutable and thread-safe — construct one <c>static readonly</c> instance per
/// product and share it.
/// </para>
/// </remarks>
public sealed class TenantManagedRoleSet
{
    private readonly HashSet<string> _managed;
    private readonly HashSet<string> _assignable;

    /// <summary>
    /// Creates a role set from the full managed roles and the
    /// tenant-owner-assignable subset.
    /// </summary>
    /// <param name="managed">Every realm role the product manages. Must be non-empty.</param>
    /// <param name="assignable">
    /// The subset a tenant owner may assign. Must be a subset of
    /// <paramref name="managed"/> (an assignable role that is not managed is a
    /// configuration error).
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="managed"/> or <paramref name="assignable"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="managed"/> is empty, or <paramref name="assignable"/> is
    /// not a subset of <paramref name="managed"/>.
    /// </exception>
    public TenantManagedRoleSet(IEnumerable<string> managed, IEnumerable<string> assignable)
    {
        ArgumentNullException.ThrowIfNull(managed);
        ArgumentNullException.ThrowIfNull(assignable);

        _managed = new HashSet<string>(managed, StringComparer.Ordinal);
        _assignable = new HashSet<string>(assignable, StringComparer.Ordinal);

        if (_managed.Count == 0)
        {
            throw new ArgumentException(
                "The managed role set must contain at least one role.", nameof(managed));
        }

        if (!_assignable.IsSubsetOf(_managed))
        {
            throw new ArgumentException(
                "Every assignable role must also be a managed role.", nameof(assignable));
        }
    }

    /// <summary>Every realm role the product manages.</summary>
    public IReadOnlyCollection<string> Managed => _managed;

    /// <summary>The subset of <see cref="Managed"/> a tenant owner may assign.</summary>
    public IReadOnlyCollection<string> Assignable => _assignable;

    /// <summary>True when <paramref name="role"/> is a managed realm role.</summary>
    public bool IsManaged(string? role) => role is not null && _managed.Contains(role);

    /// <summary>True when a tenant owner is permitted to assign <paramref name="role"/>.</summary>
    public bool IsAssignable(string? role) => role is not null && _assignable.Contains(role);

    /// <summary>
    /// True when <paramref name="roles"/> is a non-empty set in which <em>every</em>
    /// role is <see cref="IsAssignable"/>. The guard a tenant owner's invite /
    /// set-roles use cases apply before mutating a user's role set.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="roles"/> is null.</exception>
    public bool AreAllAssignable(IReadOnlyCollection<string> roles)
    {
        ArgumentNullException.ThrowIfNull(roles);
        return roles.Count > 0 && roles.All(IsAssignable);
    }
}
