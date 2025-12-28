# OnlineMenu.Identity.Abstractions

Provider-agnostic abstractions for identity and authentication in multi-tenant SaaS applications.

## Overview

This package provides interfaces and models for implementing authentication with various identity providers (Keycloak, Auth0, Azure AD, etc.) without coupling your application to a specific provider.

## Features

- 🔐 **Multiple Authentication Methods**: Username/Password, Phone OTP, Email OTP, Social Login
- 🏢 **Multi-Tenant Support**: Built-in tenant isolation
- 🔄 **Provider-Agnostic**: Easy to swap identity providers
- 📱 **OTP Support**: Configurable one-time password authentication
- ✅ **SOLID Principles**: Clean abstractions following best practices

## Installation

```bash
dotnet add package OnlineMenu.Identity.Abstractions
```

## Core Interfaces

### IIdentityProvider

Main interface for authentication operations:

```csharp
public interface IIdentityProvider
{
    Task<AuthenticationResult> AuthenticateAsync(AuthenticationRequest request, CancellationToken cancellationToken);
    Task<AuthenticationResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task<bool> RevokeTokenAsync(string token, CancellationToken cancellationToken);
    Task<UserInfo?> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken);
    Task<OtpResult> SendOtpAsync(OtpRequest request, CancellationToken cancellationToken);
    Task<AuthenticationResult> VerifyOtpAsync(string identifier, string code, Guid? tenantId, CancellationToken cancellationToken);
}
```

### IOtpService

OTP generation and validation:

```csharp
public interface IOtpService
{
    string GenerateCode(int length = 6);
    Task StoreCodeAsync(string identifier, string code, Guid tenantId, int expiryMinutes, CancellationToken cancellationToken);
    Task<bool> ValidateCodeAsync(string identifier, string code, Guid tenantId, CancellationToken cancellationToken);
    Task MarkAsUsedAsync(string identifier, string code, Guid tenantId, CancellationToken cancellationToken);
}
```

### INotificationService

Send OTP codes via SMS or email:

```csharp
public interface INotificationService
{
    Task<bool> SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken);
    Task<bool> SendEmailAsync(string email, string subject, string body, CancellationToken cancellationToken);
}
```

## Usage Example

```csharp
// Inject the identity provider
public class AuthController
{
    private readonly IIdentityProvider _identityProvider;

    public AuthController(IIdentityProvider identityProvider)
    {
        _identityProvider = identityProvider;
    }

    // Username/Password authentication
    public async Task<AuthenticationResult> Login(string username, string password)
    {
        var request = new AuthenticationRequest
        {
            AuthMethod = AuthMethod.UsernamePassword,
            Username = username,
            Password = password
        };

        return await _identityProvider.AuthenticateAsync(request);
    }

    // Phone OTP authentication
    public async Task<OtpResult> SendOtp(string phoneNumber, Guid tenantId)
    {
        var request = new OtpRequest
        {
            Identifier = phoneNumber,
            Type = OtpType.Phone,
            TenantId = tenantId
        };

        return await _identityProvider.SendOtpAsync(request);
    }

    public async Task<AuthenticationResult> VerifyOtp(string phoneNumber, string code, Guid tenantId)
    {
        return await _identityProvider.VerifyOtpAsync(phoneNumber, code, tenantId);
    }
}
```

## Models

### AuthenticationRequest

```csharp
public class AuthenticationRequest
{
    public AuthMethod AuthMethod { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? OtpCode { get; set; }
    public Guid? TenantId { get; set; }
}
```

### AuthenticationResult

```csharp
public class AuthenticationResult
{
    public bool IsSuccessful { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
    public UserInfo? UserInfo { get; set; }
    public string? ErrorMessage { get; set; }
}
```

## Configuration

```json
{
  "IdentityProvider": {
    "ProviderType": "Keycloak",
    "DefaultAuthMethod": "UsernamePassword",
    "EnableOtpAuth": true,
    "OtpCodeLength": 6,
    "OtpExpiryMinutes": 5,
    "MaxOtpAttempts": 3,
    "DevelopmentMode": false
  }
}
```

## License

MIT

## Related Packages

- `OnlineMenu.Identity.Keycloak` - Keycloak implementation
- `OnlineMenu.Identity.Auth0` - Auth0 implementation (coming soon)
- `OnlineMenu.Identity.AzureAD` - Azure AD implementation (coming soon)
