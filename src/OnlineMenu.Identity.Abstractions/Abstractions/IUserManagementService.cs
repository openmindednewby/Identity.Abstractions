using Identity.Abstractions.Models;

namespace Identity.Abstractions.Abstractions;

/// <summary>
/// Service for managing users in the identity provider
/// </summary>
public interface IUserManagementService
{
    /// <summary>
    /// Get list of users for a tenant
    /// </summary>
    Task<List<UserListItem>> GetUsersAsync(Guid? tenantId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a single user by ID
    /// </summary>
    Task<UserListItem?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new user
    /// </summary>
    Task<string> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Enable or disable a user
    /// </summary>
    Task<bool> SetUserEnabledAsync(string userId, bool enabled, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a user
    /// </summary>
    Task<bool> DeleteUserAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update user's password
    /// </summary>
    Task<bool> UpdatePasswordAsync(string userId, string newPassword, bool temporary = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Assign roles to a user
    /// </summary>
    Task<bool> AssignRolesAsync(string userId, List<string> roles, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove roles from a user
    /// </summary>
    Task<bool> RemoveRolesAsync(string userId, List<string> roles, CancellationToken cancellationToken = default);
}
