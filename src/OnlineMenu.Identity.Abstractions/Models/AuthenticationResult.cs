namespace Identity.Abstractions.Models;

/// <summary>
/// Result of an authentication attempt
/// </summary>
public class AuthenticationResult
{
  /// <summary>
  /// Whether authentication was successful
  /// </summary>
  public bool IsSuccessful { get; set; }

  /// <summary>
  /// Access token (JWT)
  /// </summary>
  public string? AccessToken { get; set; }

  /// <summary>
  /// Refresh token
  /// </summary>
  public string? RefreshToken { get; set; }

  /// <summary>
  /// ID token (OpenID Connect)
  /// </summary>
  public string? IdToken { get; set; }

  /// <summary>
  /// Token type (usually "Bearer")
  /// </summary>
  public string TokenType { get; set; } = "Bearer";

  /// <summary>
  /// Access token expiry in seconds
  /// </summary>
  public int ExpiresIn { get; set; }

  /// <summary>
  /// Refresh token expiry in seconds
  /// </summary>
  public int? RefreshExpiresIn { get; set; }

  /// <summary>
  /// User information
  /// </summary>
  public UserInfo? UserInfo { get; set; }

  /// <summary>
  /// Error message if authentication failed
  /// </summary>
  public string? ErrorMessage { get; set; }

  /// <summary>
  /// Error code for programmatic handling
  /// </summary>
  public string? ErrorCode { get; set; }

  /// <summary>
  /// Create a successful authentication result
  /// </summary>
  public static AuthenticationResult Success(
    string accessToken,
    string refreshToken,
    int expiresIn,
    UserInfo? userInfo = null,
    string? idToken = null,
    int? refreshExpiresIn = null)
  {
    return new AuthenticationResult
    {
      IsSuccessful = true,
      AccessToken = accessToken,
      RefreshToken = refreshToken,
      IdToken = idToken,
      ExpiresIn = expiresIn,
      RefreshExpiresIn = refreshExpiresIn,
      UserInfo = userInfo
    };
  }

  /// <summary>
  /// Create a failed authentication result
  /// </summary>
  public static AuthenticationResult Failure(string errorMessage, string errorCode)
  {
    return new AuthenticationResult
    {
      IsSuccessful = false,
      ErrorMessage = errorMessage,
      ErrorCode = errorCode
    };
  }
}
