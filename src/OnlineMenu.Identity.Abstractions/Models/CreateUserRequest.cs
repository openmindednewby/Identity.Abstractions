namespace OnlineMenu.Identity.Abstractions.Models;

/// <summary>
/// Request model for creating a new user
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// Username for the new user
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Email address (optional)
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Phone number (optional)
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// First name (optional)
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// Last name (optional)
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// Password for username/password authentication (optional)
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Whether the user is enabled
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Tenant ID for multi-tenant scenarios
    /// </summary>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// Roles to assign to the user
    /// </summary>
    public List<string> Roles { get; set; } = new();
}
