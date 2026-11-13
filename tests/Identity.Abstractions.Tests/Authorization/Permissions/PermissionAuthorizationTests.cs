using System.Security.Claims;
using Identity.Abstractions.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Identity.Abstractions.Tests.Authorization.Permissions;

public class PermissionAuthorizationTests
{
    private const string AuthenticationType = "test";

    [Fact]
    public async Task Authorize_WithRoleGrantingPermission_Succeeds()
    {
        using var provider = BuildServices(new[] { "funds.issue" }, ("finance-operator", new[] { "funds.issue" }));
        var user = SignedInUser("finance-operator");

        var result = await Authorize(provider, user, PermissionPolicy.For("funds.issue"));

        result.Succeeded.ShouldBeTrue();
    }

    [Fact]
    public async Task Authorize_WithoutRoleGrantingPermission_Fails()
    {
        using var provider = BuildServices(
            new[] { "deals.read", "funds.issue" },
            ("viewer", new[] { "deals.read" }));
        var user = SignedInUser("viewer");

        var result = await Authorize(provider, user, PermissionPolicy.For("funds.issue"));

        result.Succeeded.ShouldBeFalse();
    }

    [Fact]
    public async Task Authorize_WhenUserIsAnonymous_Fails()
    {
        using var provider = BuildServices(new[] { "deals.read" }, ("viewer", new[] { "deals.read" }));
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, "viewer") }));

        var result = await Authorize(provider, user, PermissionPolicy.For("deals.read"));

        result.Succeeded.ShouldBeFalse();
    }

    [Fact]
    public async Task Authorize_WhenBlockCheckReportsBlocked_FailsDespitePermission()
    {
        var blockedCheck = new FixedBlockCheck(isBlocked: true);
        using var provider = BuildServices(
            new[] { "deals.read" },
            ("viewer", new[] { "deals.read" }),
            blockedCheck);
        var user = SignedInUser("viewer");

        var result = await Authorize(provider, user, PermissionPolicy.For("deals.read"));

        result.Succeeded.ShouldBeFalse();
        result.Failure!.FailureReasons.ShouldContain(reason => reason.Message == "The principal is blocked.");
        blockedCheck.CheckedUser.ShouldBeSameAs(user);
    }

    [Fact]
    public async Task Authorize_WhenBlockCheckReportsNotBlocked_Succeeds()
    {
        using var provider = BuildServices(
            new[] { "deals.read" },
            ("viewer", new[] { "deals.read" }),
            new FixedBlockCheck(isBlocked: false));
        var user = SignedInUser("viewer");

        var result = await Authorize(provider, user, PermissionPolicy.For("deals.read"));

        result.Succeeded.ShouldBeTrue();
    }

    [Fact]
    public async Task GetPolicyAsync_WithUndeclaredPermission_ReturnsNull()
    {
        using var provider = BuildServices(new[] { "deals.read" }, ("viewer", new[] { "deals.read" }));
        var policies = provider.GetRequiredService<IAuthorizationPolicyProvider>();

        var policy = await policies.GetPolicyAsync(PermissionPolicy.For("funds.issue"));

        policy.ShouldBeNull();
    }

    [Fact]
    public async Task GetPolicyAsync_WithNamedNonPermissionPolicy_ReturnsConfiguredPolicy()
    {
        var services = new ServiceCollection().AddLogging();
        services.AddPermissionAuthorization(Map(new[] { "deals.read" }));
        services.Configure<AuthorizationOptions>(options =>
            options.AddPolicy("master-only", builder => builder.RequireRole("master")));
        using var provider = services.BuildServiceProvider();
        var policies = provider.GetRequiredService<IAuthorizationPolicyProvider>();

        var policy = await policies.GetPolicyAsync("master-only");

        policy.ShouldNotBeNull();
        policy.Requirements.ShouldNotContain(requirement => requirement is PermissionRequirement);
    }

    [Fact]
    public void TryGetPermission_WithPrefixOnly_ReturnsFalse()
    {
        var policyName = PermissionPolicy.Prefix;

        var parsed = PermissionPolicy.TryGetPermission(policyName, out var permission);

        parsed.ShouldBeFalse();
        permission.ShouldBeNull();
    }

    private static ServiceProvider BuildServices(
        string[] permissions,
        (string Role, string[] Permissions) grant,
        IBlockedPrincipalCheck? blockCheck = null)
    {
        var services = new ServiceCollection().AddLogging();
        services.AddPermissionAuthorization(Map(permissions, grant));
        if (blockCheck is not null)
        {
            services.AddSingleton(blockCheck);
        }

        return services.BuildServiceProvider();
    }

    private static PermissionMap Map(string[] permissions, params (string Role, string[] Permissions)[] grants) =>
        new(permissions, grants.ToDictionary(grant => grant.Role, grant => (IReadOnlyCollection<string>)grant.Permissions));

    private static ClaimsPrincipal SignedInUser(string role) =>
        new(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role, role) }, AuthenticationType));

    private static async Task<AuthorizationResult> Authorize(IServiceProvider provider, ClaimsPrincipal user, string policy)
    {
        using var scope = provider.CreateScope();
        var authorization = scope.ServiceProvider.GetRequiredService<IAuthorizationService>();
        return await authorization.AuthorizeAsync(user, resource: null, policy);
    }

    private sealed class FixedBlockCheck : IBlockedPrincipalCheck
    {
        private readonly bool _isBlocked;

        public FixedBlockCheck(bool isBlocked) => _isBlocked = isBlocked;

        public ClaimsPrincipal? CheckedUser { get; private set; }

        public Task<bool> IsBlockedAsync(ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            CheckedUser = user;
            return Task.FromResult(_isBlocked);
        }
    }
}
