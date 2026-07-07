using Identity.Abstractions.Authorization;
using Shouldly;
using Xunit;

namespace Identity.Abstractions.Tests.Authorization;

/// <summary>
/// Tests for <see cref="TenantManagedRoleSet"/> — the product-agnostic
/// "managed set + assignable subset + guard" convention. Logic-focused.
/// </summary>
public class TenantManagedRoleSetTests
{
    private static readonly string[] Managed =
        { "tenant-owner", "organizer", "ambassador", "door-staff" };
    private static readonly string[] Assignable =
        { "organizer", "ambassador", "door-staff" };

    private static TenantManagedRoleSet Sut() => new(Managed, Assignable);

    [Fact]
    public void Ctor_AssignableNotSubsetOfManaged_Throws()
    {
        Should.Throw<ArgumentException>(() =>
            new TenantManagedRoleSet(
                managed: new[] { "organizer" },
                assignable: new[] { "organizer", "platform-admin" }));
    }

    [Fact]
    public void Ctor_EmptyManaged_Throws()
    {
        Should.Throw<ArgumentException>(() =>
            new TenantManagedRoleSet(Array.Empty<string>(), Array.Empty<string>()));
    }

    [Fact]
    public void Ctor_NullManaged_Throws()
    {
        Should.Throw<ArgumentNullException>(() =>
            new TenantManagedRoleSet(null!, Assignable));
    }

    [Fact]
    public void IsManaged_ManagedRole_True()
    {
        Sut().IsManaged("tenant-owner").ShouldBeTrue();
    }

    [Fact]
    public void IsManaged_UnknownOrNull_False()
    {
        var sut = Sut();
        sut.IsManaged("platform-admin").ShouldBeFalse();
        sut.IsManaged(null).ShouldBeFalse();
    }

    [Fact]
    public void IsAssignable_AssignableRole_True()
    {
        Sut().IsAssignable("organizer").ShouldBeTrue();
    }

    [Fact]
    public void IsAssignable_ManagedButNotAssignable_False()
    {
        // tenant-owner is managed but a tenant owner may not assign it.
        Sut().IsAssignable("tenant-owner").ShouldBeFalse();
    }

    [Fact]
    public void AreAllAssignable_AllAssignable_True()
    {
        Sut().AreAllAssignable(new[] { "organizer", "ambassador" }).ShouldBeTrue();
    }

    [Fact]
    public void AreAllAssignable_OneNotAssignable_False()
    {
        Sut().AreAllAssignable(new[] { "organizer", "tenant-owner" }).ShouldBeFalse();
    }

    [Fact]
    public void AreAllAssignable_EmptySet_False()
    {
        Sut().AreAllAssignable(Array.Empty<string>()).ShouldBeFalse();
    }

    [Fact]
    public void ManagedAndAssignable_ExposeTheConfiguredSets()
    {
        var sut = Sut();
        sut.Managed.Count.ShouldBe(4);
        sut.Assignable.Count.ShouldBe(3);
        sut.Assignable.ShouldNotContain("tenant-owner");
    }
}
