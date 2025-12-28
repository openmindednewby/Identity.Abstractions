namespace Identity.Abstractions.Models;

/// <summary>
/// User information from the identity provider
/// </summary>
public class UserInfo
{
    /// <summary>
    /// Unique user identifier from the identity provider
    /// </summary>
    public string Sub { get; set; } = string.Empty;

    /// <summary>
    /// Username
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Email address
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Email verification status
    /// </summary>
    public bool EmailVerified { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Phone verification status
    /// </summary>
    public bool PhoneNumberVerified { get; set; }

    /// <summary>
    /// Given name (first name)
    /// </summary>
    public string? GivenName { get; set; }

    /// <summary>
    /// Family name (last name)
    /// </summary>
    public string? FamilyName { get; set; }

    /// <summary>
    /// Full name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Preferred username for display
    /// </summary>
    public string? PreferredUsername { get; set; }

    /// <summary>
    /// User roles from the identity provider
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// Tenant ID for multi-tenant scenarios
    /// </summary>
    public Guid? TenantId { get; set; }

    /// <summary>
    /// Additional custom claims
    /// </summary>
    public Dictionary<string, object>? CustomClaims { get; set; }
}
