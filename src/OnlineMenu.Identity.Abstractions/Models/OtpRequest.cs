namespace Identity.Abstractions.Models;

/// <summary>
/// Type of OTP delivery method
/// </summary>
public enum OtpType
{
  /// <summary>
  /// Send OTP via SMS
  /// </summary>
  Sms = 0,

  /// <summary>
  /// Send OTP via Email
  /// </summary>
  Email = 1
}

/// <summary>
/// Request to send an OTP code
/// </summary>
public class OtpRequest
{
  /// <summary>
  /// Type of OTP (SMS or Email)
  /// </summary>
  public OtpType Type { get; set; }

  /// <summary>
  /// Phone number or email address to send OTP to
  /// </summary>
  public string Identifier { get; set; } = string.Empty;

  /// <summary>
  /// Tenant ID for multi-tenant scenarios
  /// </summary>
  public Guid? TenantId { get; set; }

  /// <summary>
  /// Custom message template (optional)
  /// </summary>
  public string? MessageTemplate { get; set; }
}
