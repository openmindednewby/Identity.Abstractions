namespace Identity.Abstractions.Models;

/// <summary>
/// Unified authentication request supporting multiple auth methods
/// </summary>
public class AuthenticationRequest
{
  /// <summary>
  /// The authentication method to use
  /// </summary>
  public AuthMethod AuthMethod { get; set; }

  /// <summary>
  /// Username for username/password authentication
  /// </summary>
  public string? Username { get; set; }

  /// <summary>
  /// Password for username/password authentication
  /// </summary>
  public string? Password { get; set; }

  /// <summary>
  /// Phone number for phone OTP authentication
  /// </summary>
  public string? PhoneNumber { get; set; }

  /// <summary>
  /// Email for email OTP authentication
  /// </summary>
  public string? Email { get; set; }

  /// <summary>
  /// OTP code for verification
  /// </summary>
  public string? OtpCode { get; set; }

  /// <summary>
  /// Social provider token (Google, Apple, etc.)
  /// </summary>
  public string? SocialToken { get; set; }

  /// <summary>
  /// Social provider name (google, apple, facebook, etc.)
  /// </summary>
  public string? SocialProvider { get; set; }

  /// <summary>
  /// Tenant ID for multi-tenant scenarios
  /// </summary>
  public Guid? TenantId { get; set; }

  /// <summary>
  /// Client information (device, browser, etc.) for logging
  /// </summary>
  public string? ClientInfo { get; set; }

  /// <summary>
  /// IP address for security logging
  /// </summary>
  public string? IpAddress { get; set; }
}
