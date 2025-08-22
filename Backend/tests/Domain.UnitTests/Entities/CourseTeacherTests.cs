using Afama.Go.Api.Domain.Entities;
using Afama.Go.Api.Domain.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace Afama.Go.Api.Domain.UnitTests.Entities;

public class CourseTeacherTests
{
    [Test]
    public void CourseTeacherType_ShouldDefaultToMain()
    {
        var teacher = new CourseTeacher();

        teacher.CourseTeacherType.Should().Be(CourseTeacherType.Main);
    }
}
