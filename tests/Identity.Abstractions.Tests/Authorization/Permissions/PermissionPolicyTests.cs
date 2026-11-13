using Identity.Abstractions.Authorization.Permissions;
using Shouldly;
using Xunit;

namespace Identity.Abstractions.Tests.Authorization.Permissions;

public class PermissionPolicyTests
{
    [Fact]
    public void TryGetPermission_WithPrefixOnly_ReturnsFalse()
    {
        var policyName = PermissionPolicy.Prefix;

        var parsed = PermissionPolicy.TryGetPermission(policyName, out var permission);

        parsed.ShouldBeFalse();
        permission.ShouldBeNull();
    }

    [Fact]
    public void TryGetPermission_WithPermissionPolicyName_ReturnsPermission()
    {
        var policyName = "perm:funds.issue";

        var parsed = PermissionPolicy.TryGetPermission(policyName, out var permission);

        parsed.ShouldBeTrue();
        permission.ShouldBe("funds.issue");
    }
}
