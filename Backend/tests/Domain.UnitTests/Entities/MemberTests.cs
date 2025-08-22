using Afama.Go.Api.Domain.Entities;
using Afama.Go.Api.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace Afama.Go.Api.Domain.UnitTests.Entities;

public class MemberTests
{
    [Test]
    public void Constructor_ShouldInitializeCollections()
    {
        var member = new Member();

        member.Clubs.Should().NotBeNull().And.BeEmpty();
    }

    [Test]
    public void MemberType_ShouldDefaultToStudent()
    {
        var member = new Member();

        member.MemberType.Should().Be(MemberType.Student);
    }
}
