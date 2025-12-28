namespace OnlineMenu.Identity.Abstractions.Exceptions;

/// <summary>
/// Base exception for identity-related errors
/// </summary>
public class IdentityException : Exception
{
    /// <summary>
    /// Error code for programmatic handling
    /// </summary>
    public string? ErrorCode { get; set; }

    public IdentityException() { }

    public IdentityException(string message) : base(message) { }

    public IdentityException(string message, Exception innerException)
        : base(message, innerException) { }

    public IdentityException(string message, string errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
    }
}

/// <summary>
/// Exception thrown when authentication fails
/// </summary>
public class AuthenticationException : IdentityException
{
    public AuthenticationException() { }

    public AuthenticationException(string message) : base(message) { }

    public AuthenticationException(string message, string errorCode)
        : base(message, errorCode) { }

    public AuthenticationException(string message, Exception innerException)
        : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when credentials are invalid
/// </summary>
public class InvalidCredentialsException : AuthenticationException
{
    public InvalidCredentialsException()
        : base("Invalid username or password", "INVALID_CREDENTIALS") { }

    public InvalidCredentialsException(string message)
        : base(message, "INVALID_CREDENTIALS") { }
}

/// <summary>
/// Exception thrown when OTP code is invalid or expired
/// </summary>
public class OtpException : AuthenticationException
{
    public OtpException(string message) : base(message, "OTP_ERROR") { }

    public OtpException(string message, string errorCode) : base(message, errorCode) { }
}

/// <summary>
/// Exception thrown when OTP code has expired
/// </summary>
public class OtpExpiredException : OtpException
{
    public OtpExpiredException()
        : base("OTP code has expired", "OTP_EXPIRED") { }
}

/// <summary>
/// Exception thrown when too many OTP attempts have been made
/// </summary>
public class TooManyAttemptsException : OtpException
{
    public TooManyAttemptsException()
        : base("Too many verification attempts. Please request a new code.", "TOO_MANY_ATTEMPTS") { }

    public TooManyAttemptsException(int attemptsRemaining)
        : base($"Invalid code. {attemptsRemaining} attempts remaining.", "TOO_MANY_ATTEMPTS") { }
}

/// <summary>
/// Exception thrown when token is invalid or expired
/// </summary>
public class InvalidTokenException : IdentityException
{
    public InvalidTokenException()
        : base("Token is invalid or expired", "INVALID_TOKEN") { }

    public InvalidTokenException(string message)
        : base(message, "INVALID_TOKEN") { }
}
