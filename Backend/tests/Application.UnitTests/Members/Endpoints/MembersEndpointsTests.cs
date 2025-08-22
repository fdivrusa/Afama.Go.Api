using Afama.Go.Api.Application.Members.Commands;
using Afama.Go.Api.Application.Members.Queries.GetMemberDetails;
using Afama.Go.Api.Application.Members.Queries.GetMembers;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using NUnit.Framework;
using MembersEndpoint = Afama.Go.Api.Host.Endpoints.Members;

namespace Afama.Go.Api.Application.UnitTests.Members.Endpoints;

[TestFixture]
public class MembersEndpointsTests
{
    private Mock<ISender> _mockSender;
    private MembersEndpoint _membersEndpoint;

    [SetUp]
    public void SetUp()
    {
        _mockSender = new Mock<ISender>();
        _membersEndpoint = new MembersEndpoint();
    }

    [Test]
    public async Task GetMembers_Should_Return_Ok_With_Members_List()
    {
        // Arrange
        var query = new GetMembersQuery
        {
            FirstName = "John",
            LastName = "Doe"
        };

        var expectedMembers = new List<MemberBriefDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "+1234567890",
                MemberType = "Student"
            }
        };

        _mockSender.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedMembers);

        // Act
        var result = await _membersEndpoint.GetMembers(_mockSender.Object, query);

        // Assert
        result.Should().BeOfType<Ok<IEnumerable<MemberBriefDto>>>();
        result.Value.Should().BeEquivalentTo(expectedMembers);
        _mockSender.Verify(x => x.Send(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetMembers_Should_Return_Empty_List_When_No_Members_Found()
    {
        // Arrange
        var query = new GetMembersQuery();
        var expectedMembers = new List<MemberBriefDto>();

        _mockSender.Setup(x => x.Send(query, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedMembers);

        // Act
        var result = await _membersEndpoint.GetMembers(_mockSender.Object, query);

        // Assert
        result.Should().BeOfType<Ok<IEnumerable<MemberBriefDto>>>();
        result.Value.Should().BeEmpty();
        _mockSender.Verify(x => x.Send(query, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetMemberDetails_Should_Return_Ok_With_Member_Details()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var expectedMemberDetails = new MemberDetailsDto
        {
            Id = memberId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "+1234567890",
            MemberType = "Student",
            BirthDate = new DateTime(1990, 1, 1),
            CreatedAt = DateTime.UtcNow.AddDays(-30),
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };

        _mockSender.Setup(x => x.Send(It.Is<GetMemberDetailsQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedMemberDetails);

        // Act
        var result = await _membersEndpoint.GetMemberDetails(_mockSender.Object, memberId);

        // Assert
        result.Should().BeOfType<Results<Ok<MemberDetailsDto>, NotFound>>();
        var okResult = result.Result as Ok<MemberDetailsDto>;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(expectedMemberDetails);

        _mockSender.Verify(x => x.Send(It.Is<GetMemberDetailsQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetMemberDetails_Should_Create_Correct_Query()
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var expectedMemberDetails = new MemberDetailsDto
        {
            Id = memberId,
            FirstName = "John",
            LastName = "Doe"
        };

        _mockSender.Setup(x => x.Send(It.IsAny<GetMemberDetailsQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(expectedMemberDetails);

        // Act
        await _membersEndpoint.GetMemberDetails(_mockSender.Object, memberId);

        // Assert
        _mockSender.Verify(x => x.Send(
            It.Is<GetMemberDetailsQuery>(q => q.MemberId == memberId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task CreateMember_Should_Return_Created_With_Member_Id()
    {
        // Arrange
        var command = new CreateMemberCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "+1234567890",
            MemberType = Domain.Enums.MemberType.Student
        };

        var expectedMemberId = Guid.NewGuid();

        _mockSender.Setup(x => x.Send(It.Is<CreateMemberCommand>(c =>
                c.FirstName == command.FirstName &&
                c.LastName == command.LastName &&
                c.Email == command.Email &&
                c.PhoneNumber == command.PhoneNumber &&
                c.MemberType == command.MemberType &&
                c.BirthDate == command.BirthDate &&
                c.KnownPathologies == command.KnownPathologies),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedMemberId);

        // Act
        var result = await _membersEndpoint.CreateMember(_mockSender.Object, command);

        // Assert
        result.Should().BeOfType<Created<Guid>>();
        result.Value.Should().Be(expectedMemberId);
        result.Location.Should().Be($"/{nameof(Members)}/{expectedMemberId}");
        _mockSender.Verify(x => x.Send(It.IsAny<CreateMemberCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task CreateMember_Should_Call_Sender_With_Exact_Command()
    {
        // Arrange
        var command = new CreateMemberCommand
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            PhoneNumber = "+0987654321",
            MemberType = Domain.Enums.MemberType.Teacher,
            BirthDate = new DateTime(1985, 6, 15),
            KnownPathologies = "Diabetes"
        };

        var memberId = Guid.NewGuid();
        _mockSender.Setup(x => x.Send(It.Is<CreateMemberCommand>(c =>
                c.FirstName == command.FirstName &&
                c.LastName == command.LastName &&
                c.Email == command.Email &&
                c.PhoneNumber == command.PhoneNumber &&
                c.MemberType == command.MemberType &&
                c.BirthDate == command.BirthDate &&
                c.KnownPathologies == command.KnownPathologies),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(memberId);

        // Act
        await _membersEndpoint.CreateMember(_mockSender.Object, command);

        // Assert
        _mockSender.Verify(x => x.Send(It.Is<CreateMemberCommand>(c =>
                c.FirstName == command.FirstName &&
                c.LastName == command.LastName &&
                c.Email == command.Email &&
                c.PhoneNumber == command.PhoneNumber &&
                c.MemberType == command.MemberType &&
                c.BirthDate == command.BirthDate &&
                c.KnownPathologies == command.KnownPathologies),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task CreateMember_Should_Return_Correct_Location_Format()
    {
        // Arrange
        var command = new CreateMemberCommand
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            PhoneNumber = "+1111111111",
            MemberType = Domain.Enums.MemberType.Other
        };

        var memberId = Guid.NewGuid();
        _mockSender.Setup(x => x.Send(It.IsAny<CreateMemberCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(memberId);

        // Act
        var result = await _membersEndpoint.CreateMember(_mockSender.Object, command);

        // Assert
        result.Location.Should().Be($"/Members/{memberId}");
    }

    [Test]
    public async Task UpdateMember_Should_Call_Sender_With_Command_And_Return_NoContent()
    {
        var id = Guid.NewGuid();
        var command = new UpdateMemberCommand
        {
            FirstName = "New",
            LastName = "Name",
            Email = "new@example.com",
            PhoneNumber = "+1",
            MemberType = Domain.Enums.MemberType.Student
        };

        _mockSender.Setup(x => x.Send(It.Is<UpdateMemberCommand>(c =>
                c.Id == id &&
                c.FirstName == command.FirstName &&
                c.LastName == command.LastName &&
                c.Email == command.Email &&
                c.PhoneNumber == command.PhoneNumber &&
                c.MemberType == command.MemberType &&
                c.BirthDate == command.BirthDate &&
                c.KnownPathologies == command.KnownPathologies),
            It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _membersEndpoint.UpdateMember(_mockSender.Object, id, command);

        result.Should().BeOfType<Results<NoContent, NotFound>>();
        _mockSender.Verify(x => x.Send(It.Is<UpdateMemberCommand>(c => c.Id == id), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task DeleteMember_Should_Call_Sender_With_Command_And_Return_NoContent()
    {
        var id = Guid.NewGuid();

        _mockSender.Setup(x => x.Send(It.Is<DeleteMemberCommand>(c => c.Id == id), It.IsAny<CancellationToken>()))
                   .Returns(Task.CompletedTask);

        var result = await _membersEndpoint.DeleteMember(_mockSender.Object, id);

        result.Should().BeOfType<Results<NoContent, NotFound>>();
        _mockSender.Verify(x => x.Send(It.Is<DeleteMemberCommand>(c => c.Id == id), It.IsAny<CancellationToken>()), Times.Once);
    }
}

