using Afama.Go.Api.Application.Members.Queries.GetMemberDetails;
using Afama.Go.Api.Domain.Entities;
using Afama.Go.Api.Domain.Enums;
using Afama.Go.Api.Infrastructure.Data;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Afama.Go.Api.Application.UnitTests.Members.Queries;

[TestFixture]
public class GetMemberDetailsQueryHandlerTests
{
    private ApplicationDbContext _context;
    private IMapper _mapper;
    private GetMemberDetailsQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(GetMemberDetailsQuery).Assembly);
        });
        _mapper = configuration.CreateMapper();

        _handler = new GetMemberDetailsQueryHandler(_context, _mapper);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task Handle_Should_Return_MemberDetailsDto_When_Member_Exists()
    {
        // Arrange
        var member = new Member
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "+123",
            MemberType = MemberType.Student,
            BirthDate = new DateTime(1990, 1, 1),
            Created = DateTimeOffset.UtcNow.AddDays(-10),
            LastModified = DateTimeOffset.UtcNow.AddDays(-1)
        };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var query = new GetMemberDetailsQuery { MemberId = member.Id };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(new MemberDetailsDto
        {
            Id = member.Id,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email,
            PhoneNumber = member.PhoneNumber,
            MemberType = member.MemberType.ToString(),
            BirthDate = member.BirthDate,
            CreatedAt = member.Created,
            UpdatedAt = member.LastModified
        });
    }

    [Test]
    public async Task Handle_Should_Throw_NotFoundException_When_Member_Not_Found()
    {
        // Arrange
        var query = new GetMemberDetailsQuery { MemberId = Guid.NewGuid() };

        // Act
        var act = () => _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
