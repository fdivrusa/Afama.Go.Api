using Afama.Go.Api.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Afama.Go.Api.Domain.UnitTests.Entities;

public class ClubTests
{
    [Test]
    public void Constructor_ShouldInitializeCollections()
    {
        var club = new Club();

        club.Courses.Should().NotBeNull().And.BeEmpty();
        club.Members.Should().NotBeNull().And.BeEmpty();
    }
}
