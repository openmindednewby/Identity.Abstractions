using Identity.Abstractions.Authorization.Permissions;
using Shouldly;
using Xunit;

namespace Identity.Abstractions.Tests.Authorization.Permissions;

public class PermissionMapTests
{
    [Fact]
    public void Ctor_WithRoleMappingUndeclaredPermission_Throws()
    {
        var rolePermissions = new Dictionary<string, IReadOnlyCollection<string>>
        {
            ["viewer"] = new[] { "deals.read", "funds.issue" },
        };

        Should.Throw<ArgumentException>(() => new PermissionMap(new[] { "deals.read" }, rolePermissions));
    }

    [Fact]
    public void Ctor_WithNoPermissions_Throws()
    {
        var rolePermissions = new Dictionary<string, IReadOnlyCollection<string>>();

        Should.Throw<ArgumentException>(() => new PermissionMap(Array.Empty<string>(), rolePermissions));
    }

    [Fact]
    public void Ctor_WithBlankPermission_Throws()
    {
        var rolePermissions = new Dictionary<string, IReadOnlyCollection<string>>();

        Should.Throw<ArgumentException>(() => new PermissionMap(new[] { "deals.read", " " }, rolePermissions));
    }

    [Fact]
    public void Grants_WithRoleHoldingPermission_ReturnsTrue()
    {
        var map = new PermissionMap(
            new[] { "deals.read", "funds.issue" },
            new Dictionary<string, IReadOnlyCollection<string>> { ["finance-operator"] = new[] { "funds.issue" } });

        var granted = map.Grants(new[] { "viewer", "finance-operator" }, "funds.issue");

        granted.ShouldBeTrue();
    }

    [Fact]
    public void Grants_WithoutRoleHoldingPermission_ReturnsFalse()
    {
        var map = new PermissionMap(
            new[] { "deals.read", "funds.issue" },
            new Dictionary<string, IReadOnlyCollection<string>> { ["viewer"] = new[] { "deals.read" } });

        var granted = map.Grants(new[] { "viewer", "unknown-role" }, "funds.issue");

        granted.ShouldBeFalse();
    }

    [Fact]
    public void Grants_WhenPermissionCaseDiffers_ReturnsFalse()
    {
        var map = new PermissionMap(
            new[] { "deals.read" },
            new Dictionary<string, IReadOnlyCollection<string>> { ["viewer"] = new[] { "deals.read" } });

        var granted = map.Grants(new[] { "viewer" }, "Deals.Read");

        granted.ShouldBeFalse();
    }

    [Fact]
    public void PermissionsFor_WithSeveralRoles_ReturnsUnionOfTheirPermissions()
    {
        var map = new PermissionMap(
            new[] { "deals.read", "funds.issue", "funds.approve" },
            new Dictionary<string, IReadOnlyCollection<string>>
            {
                ["viewer"] = new[] { "deals.read" },
                ["approver"] = new[] { "deals.read", "funds.approve" },
                ["finance-operator"] = new[] { "funds.issue" },
            });

        var permissions = map.PermissionsFor(new[] { "viewer", "approver", "unknown-role" });

        permissions.ShouldBe(new[] { "deals.read", "funds.approve" }, ignoreOrder: true);
    }

    [Fact]
    public void IsDeclared_WithNullOrUndeclaredName_ReturnsFalse()
    {
        var map = new PermissionMap(
            new[] { "deals.read" },
            new Dictionary<string, IReadOnlyCollection<string>>());

        var declared = new[] { map.IsDeclared(null), map.IsDeclared("funds.issue"), map.IsDeclared("deals.read") };

        declared.ShouldBe(new[] { false, false, true });
    }

    [Fact]
    public void ToMatrix_WithDeclaredOrder_ListsRolesAndPermissionsInThatOrder()
    {
        var map = new PermissionMap(
            new[] { "funds.issue", "deals.read", "users.block" },
            new Dictionary<string, IReadOnlyCollection<string>>
            {
                ["tenant-admin"] = new[] { "users.block", "deals.read", "funds.issue" },
                ["viewer"] = new[] { "deals.read" },
            });

        var matrix = map.ToMatrix();

        matrix.Roles.ShouldBe(new[] { "tenant-admin", "viewer" });
        matrix.Permissions.ShouldBe(new[] { "funds.issue", "deals.read", "users.block" });
        matrix.Rows[0].Permissions.ShouldBe(new[] { "funds.issue", "deals.read", "users.block" });
        matrix.Rows[1].Role.ShouldBe("viewer");
        matrix.Rows[1].Permissions.ShouldBe(new[] { "deals.read" });
    }
}
