using OnlineMenu.Identity.Abstractions.Models;

namespace OnlineMenu.Identity.Abstractions.Abstractions;

/// <summary>
/// Service for OTP generation and validation
/// </summary>
public interface IOtpService
{
    /// <summary>
    /// Generate an OTP code
    /// </summary>
    /// <param name="length">Length of the OTP code</param>
    /// <returns>Generated OTP code</returns>
    string GenerateCode(int length = 6);

    /// <summary>
    /// Store an OTP code for later verification
    /// </summary>
    /// <param name="identifier">Phone/email identifier</param>
    /// <param name="code">The OTP code</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="expiryMinutes">Expiry time in minutes</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task StoreCodeAsync(
        string identifier,
        string code,
        Guid tenantId,
        int expiryMinutes,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Validate an OTP code
    /// </summary>
    /// <param name="identifier">Phone/email identifier</param>
    /// <param name="code">The OTP code to validate</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if code is valid</returns>
    Task<bool> ValidateCodeAsync(
        string identifier,
        string code,
        Guid tenantId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Mark an OTP code as used
    /// </summary>
    /// <param name="identifier">Phone/email identifier</param>
    /// <param name="code">The OTP code</param>
    /// <param name="tenantId">Tenant identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task MarkAsUsedAsync(
        string identifier,
        string code,
        Guid tenantId,
        CancellationToken cancellationToken = default);
}
