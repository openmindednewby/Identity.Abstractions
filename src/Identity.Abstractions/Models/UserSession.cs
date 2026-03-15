namespace Identity.Abstractions.Models;

/// <summary>
/// Represents an active user session from the identity provider.
/// </summary>
public class UserSession
{
    /// <summary>
    /// Unique session identifier.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// IP address of the client that initiated the session.
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// Unix timestamp (seconds) when the session started.
    /// </summary>
    public long Start { get; set; }

    /// <summary>
    /// Unix timestamp (seconds) of the last activity in the session.
    /// </summary>
    public long LastAccess { get; set; }

    /// <summary>
    /// List of client application names that have been used in this session.
    /// </summary>
    public List<string> Clients { get; set; } = new();
}
