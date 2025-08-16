using Afama.Go.Api.Application.Common.Interfaces;
using Moq;
using NUnit.Framework;

namespace Afama.Go.Api.Application.UnitTests.Common.Behaviours;
public class RequestLoggerTests
{
    private Mock<IUser> _user = null!;

    [SetUp]
    public void Setup()
    {
        _user = new Mock<IUser>();
    }
}
