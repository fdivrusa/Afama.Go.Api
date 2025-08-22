using Afama.Go.Api.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace Afama.Go.Api.Domain.UnitTests.Entities;

public class CourseTests
{
    [Test]
    public void Constructor_ShouldInitializeCollections()
    {
        var course = new Course();

        course.Teachers.Should().NotBeNull().And.BeEmpty();
    }
}
