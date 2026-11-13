# Identity.Abstractions

[![NuGet](https://img.shields.io/nuget/v/Identity.Abstractions.svg)](https://www.nuget.org/packages/Identity.Abstractions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Provider-agnostic authentication interfaces and contracts for identity services

## Installation

```bash
dotnet add package Identity.Abstractions
```

## Description

Provider-agnostic authentication interfaces and contracts for identity services

## Documentation

See the [NuGet package page](https://www.nuget.org/packages/Identity.Abstractions) for full documentation.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Related Packages

- [Identity.Keycloak](https://www.nuget.org/packages/Identity.Keycloak)
- [MultiTenant.NotificationProviders](https://www.nuget.org/packages/MultiTenant.NotificationProviders)
- [OtpAuthentication](https://www.nuget.org/packages/OtpAuthentication)

## Support

- Issues: [GitHub Issues](https://github.com/openmindednewby/Identity.Abstractions/issues)
- Discussions: [GitHub Discussions](https://github.com/openmindednewby/Identity.Abstractions/discussions)

## How to Publish

```bash
cd C:\desktopContents\projects\SaaS\NuGetPackages\Identity.Abstractions
.\publish.ps1 -ApiKey XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX -Bump patch  # Bug fixes
.\publish.ps1 -ApiKey XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX -Bump minor  # New features
.\publish.ps1 -ApiKey XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX -Bump major  # Breaking changes
```
## Permissions

`Identity.Abstractions.Authorization.Permissions`: a product declares named permissions and a role -> permissions `PermissionMap`; `PermissionPolicy.For("funds.issue")` gives the policy name `perm:funds.issue`; `IBlockedPrincipalCheck` reports a blocked user, organization or tenant; `map.ToMatrix()` returns the role x permission read model for a UI. The ASP.NET Core wiring (policy provider, handlers, `AddPermissionAuthorization`) is in the `Identity.Authorization.AspNetCore` package.
