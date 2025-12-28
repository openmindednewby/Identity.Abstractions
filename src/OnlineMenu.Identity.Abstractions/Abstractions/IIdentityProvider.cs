using OnlineMenu.Identity.Abstractions.Models;

namespace OnlineMenu.Identity.Abstractions.Abstractions;

/// <summary>
/// Abstraction for identity provider operations
/// This interface allows swapping between different identity providers (Keycloak, Auth0, Azure AD, etc.)
/// </summary>
public interface IIdentityProvider
{
    /// <summary>
    /// Authenticate a user with the provided credentials
    /// </summary>
    /// <param name="request">Authentication request with credentials</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result with tokens and user info</returns>
    Task<AuthenticationResult> AuthenticateAsync(
        AuthenticationRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Refresh an access token using a refresh token
    /// </summary>
    /// <param name="refreshToken">The refresh token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>New authentication result with refreshed tokens</returns>
    Task<AuthenticationResult> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Revoke a token (logout)
    /// </summary>
    /// <param name="token">The token to revoke (access or refresh token)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if revocation was successful</returns>
    Task<bool> RevokeTokenAsync(
        string token,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get user information from an access token
    /// </summary>
    /// <param name="accessToken">The access token</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User information</returns>
    Task<UserInfo?> GetUserInfoAsync(
        string accessToken,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Send an OTP code to a phone number or email
    /// </summary>
    /// <param name="request">OTP request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>OTP result</returns>
    Task<OtpResult> SendOtpAsync(
        OtpRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Verify an OTP code
    /// </summary>
    /// <param name="identifier">Phone number or email</param>
    /// <param name="code">The OTP code to verify</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication result if verification successful</returns>
    Task<AuthenticationResult> VerifyOtpAsync(
        string identifier,
        string code,
        Guid? tenantId = null,
        CancellationToken cancellationToken = default);
}
