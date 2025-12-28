namespace OnlineMenu.Identity.Abstractions.Configuration;

/// <summary>
/// Configuration options for identity provider
/// </summary>
public class IdentityProviderOptions
{
  /// <summary>
  /// Configuration section name in appsettings.json
  /// </summary>
  public const string SectionName = "IdentityProvider";

  /// <summary>
  /// Whether OTP authentication is enabled
  /// </summary>
  public bool EnableOtpAuth { get; set; } = true;

  /// <summary>
  /// Default OTP code length (4-10 digits)
  /// </summary>
  public int OtpCodeLength { get; set; } = 6;

  /// <summary>
  /// Default OTP expiry time in minutes
  /// </summary>
  public int OtpExpiryMinutes { get; set; } = 5;

  /// <summary>
  /// Development mode - returns OTP code in API response (DO NOT use in production!)
  /// </summary>
  public bool DevelopmentMode { get; set; } = false;
}
