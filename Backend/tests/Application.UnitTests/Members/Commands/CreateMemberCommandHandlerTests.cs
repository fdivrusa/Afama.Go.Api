using Afama.Go.Api.Application.Common.Interfaces;
using Afama.Go.Api.Application.Members.Commands;
using Afama.Go.Api.Domain.Entities;
using Afama.Go.Api.Domain.Enums;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace Afama.Go.Api.Application.UnitTests.Members.Commands;

[TestFixture]
public class CreateMemberCommandHandlerTests
{
    private Mock<IApplicationDbContext> _mockContext;
    private Mock<DbSet<Member>> _mockMembersDbSet;
    private CreateMemberCommandHandler _handler;
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mockMembersDbSet = new Mock<DbSet<Member>>();
        
        _mockContext.Setup(x => x.Members).Returns(_mockMembersDbSet.Object);
        _mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

        var configuration = new MapperConfiguration(cfg => cfg.CreateMap<CreateMemberCommand, Member>());
        _mapper = configuration.CreateMapper();

        _handler = new CreateMemberCommandHandler(_mockContext.Object, _mapper);
    }

    [Test]
    public async Task Handle_Should_Create_Member_With_Correct_Properties()
    {
        // Arrange
        var command = new CreateMemberCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "+1234567890",
            MemberType = MemberType.Student,
            BirthDate = new DateTime(1990, 1, 1),
            KnownPathologies = "None"
        };

        Member? capturedMember = null;
        _mockMembersDbSet.Setup(x => x.Add(It.IsAny<Member>()))
                        .Callback<Member>(m => 
                        {
                            if (m.Id == Guid.Empty)
                                m.Id = Guid.NewGuid();
                            capturedMember = m;
                        });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        capturedMember.Should().NotBeNull();
        capturedMember!.FirstName.Should().Be(command.FirstName);
        capturedMember.LastName.Should().Be(command.LastName);
        capturedMember.Email.Should().Be(command.Email);
        capturedMember.PhoneNumber.Should().Be(command.PhoneNumber);
        capturedMember.MemberType.Should().Be(command.MemberType);
        capturedMember.BirthDate.Should().Be(command.BirthDate);
        capturedMember.KnownPathologies.Should().Be(command.KnownPathologies);
        capturedMember.Id.Should().NotBeEmpty();
        result.Should().Be(capturedMember.Id);
    }

    [Test]
    public async Task Handle_Should_Add_Member_To_DbSet()
    {
        // Arrange
        var command = new CreateMemberCommand
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            PhoneNumber = "+0987654321",
            MemberType = MemberType.Teacher
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockMembersDbSet.Verify(x => x.Add(It.IsAny<Member>()), Times.Once);
    }

    [Test]
    public async Task Handle_Should_Save_Changes_To_Database()
    {
        // Arrange
        var command = new CreateMemberCommand
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            PhoneNumber = "+1111111111",
            MemberType = MemberType.Other
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_Should_Return_Member_Id()
    {
        // Arrange
        var command = new CreateMemberCommand
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            PhoneNumber = "+1111111111",
            MemberType = MemberType.Parent
        };

        Member? capturedMember = null;
        _mockMembersDbSet.Setup(x => x.Add(It.IsAny<Member>()))
                        .Callback<Member>(m => capturedMember = m);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(capturedMember!.Id);
    }

    [Test]
    public async Task Handle_Should_Handle_Null_Optional_Properties()
    {
        // Arrange
        var command = new CreateMemberCommand
        {
            FirstName = "Minimal",
            LastName = "User",
            Email = "minimal@example.com",
            PhoneNumber = "+1111111111",
            MemberType = MemberType.Student,
            BirthDate = null,
            KnownPathologies = null
        };

        Member? capturedMember = null;
        _mockMembersDbSet.Setup(x => x.Add(It.IsAny<Member>()))
                        .Callback<Member>(m =>
                        {
                            if (m.Id == Guid.Empty)
                                m.Id = Guid.NewGuid();
                            capturedMember = m;
                        });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty();
        capturedMember.Should().NotBeNull();
        capturedMember!.BirthDate.Should().BeNull();
        capturedMember.KnownPathologies.Should().BeNull();

    }

    [Test]
    public async Task Handle_Should_Use_Provided_CancellationToken()
    {
        // Arrange
        var command = new CreateMemberCommand
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            PhoneNumber = "+1111111111",
            MemberType = MemberType.Assistant
        };

        var cancellationToken = new CancellationToken();

        // Act
        await _handler.Handle(command, cancellationToken);

        // Assert
        _mockContext.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
    }
}
