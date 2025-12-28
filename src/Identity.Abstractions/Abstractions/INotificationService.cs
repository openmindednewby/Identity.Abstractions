namespace Identity.Abstractions.Abstractions;

/// <summary>
/// Service for sending notifications (SMS, Email)
/// </summary>
public interface INotificationService
{
  /// <summary>
  /// Send an SMS message
  /// </summary>
  /// <param name="phoneNumber">Phone number in E.164 format (e.g., +1234567890)</param>
  /// <param name="message">Message content</param>
  /// <param name="cancellationToken">Cancellation token</param>
  /// <returns>True if sent successfully</returns>
  Task<bool> SendSmsAsync(
    string phoneNumber,
    string message,
    CancellationToken cancellationToken = default);

  /// <summary>
  /// Send an email message
  /// </summary>
  /// <param name="email">Email address</param>
  /// <param name="subject">Email subject</param>
  /// <param name="body">Email body</param>
  /// <param name="cancellationToken">Cancellation token</param>
  /// <returns>True if sent successfully</returns>
  Task<bool> SendEmailAsync(
    string email,
    string subject,
    string body,
    CancellationToken cancellationToken = default);
}
