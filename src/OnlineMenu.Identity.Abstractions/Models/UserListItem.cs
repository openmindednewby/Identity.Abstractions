namespace OnlineMenu.Identity.Abstractions.Models;

/// <summary>
/// User list item for displaying users
/// </summary>
public class UserListItem
{
    /// <summary>
    /// Unique user identifier
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Email address
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Whether the user is enabled
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// User creation timestamp
    /// </summary>
    public DateTime? CreatedTimestamp { get; set; }

    /// <summary>
    /// Roles assigned to the user
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// Tenant ID the user belongs to
    /// </summary>
    public string? TenantId { get; set; }
}
