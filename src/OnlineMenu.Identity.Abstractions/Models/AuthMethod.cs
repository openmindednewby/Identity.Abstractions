namespace OnlineMenu.Identity.Abstractions.Models;

/// <summary>
/// Supported authentication methods
/// </summary>
public enum AuthMethod
{
    /// <summary>
    /// Traditional username and password authentication
    /// </summary>
    UsernamePassword = 0,

    /// <summary>
    /// Phone number with One-Time Password (OTP)
    /// </summary>
    PhoneOtp = 1,

    /// <summary>
    /// Email with One-Time Password (OTP)
    /// </summary>
    EmailOtp = 2,

    /// <summary>
    /// Social login (Google, Apple, Facebook, etc.)
    /// </summary>
    Social = 3
}
