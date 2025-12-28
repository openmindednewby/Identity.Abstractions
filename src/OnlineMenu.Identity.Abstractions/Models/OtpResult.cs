namespace OnlineMenu.Identity.Abstractions.Models;

/// <summary>
/// Result of an OTP operation
/// </summary>
public class OtpResult
{
    /// <summary>
    /// Indicates if the operation was successful
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// OTP code (only included in development/test environments)
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Expiration time in seconds
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Error message if operation failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Number of attempts remaining before lockout
    /// </summary>
    public int? AttemptsRemaining { get; set; }

    /// <summary>
    /// Helper method to create a successful result
    /// </summary>
    public static OtpResult Success(int expiresIn, string? code = null)
    {
        return new OtpResult
        {
            IsSuccessful = true,
            ExpiresIn = expiresIn,
            Code = code
        };
    }

    /// <summary>
    /// Helper method to create a failed result
    /// </summary>
    public static OtpResult Failure(string errorMessage, int? attemptsRemaining = null)
    {
        return new OtpResult
        {
            IsSuccessful = false,
            ErrorMessage = errorMessage,
            AttemptsRemaining = attemptsRemaining
        };
    }
}
