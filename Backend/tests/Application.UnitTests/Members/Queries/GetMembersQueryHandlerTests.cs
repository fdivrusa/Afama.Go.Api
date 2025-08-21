using Afama.Go.Api.Application.Members.Queries.GetMembers;
using Afama.Go.Api.Domain.Entities;
using Afama.Go.Api.Domain.Enums;
using Afama.Go.Api.Infrastructure.Data;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Afama.Go.Api.Application.UnitTests.Members.Queries;

[TestFixture]
public class GetMembersQueryHandlerTests
{
    private ApplicationDbContext _context;
    private IMapper _mapper;
    private GetMembersQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(GetMembersQuery).Assembly);
        });
        _mapper = configuration.CreateMapper();

        _handler = new GetMembersQueryHandler(_context, _mapper);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task Handle_Should_Filter_FirstName_Ignoring_Case()
    {
        // Arrange
        _context.Members.AddRange(
            new Member
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice@example.com",
                PhoneNumber = "+123",
                MemberType = MemberType.Student
            },
            new Member
            {
                Id = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "Jones",
                Email = "bob@example.com",
                PhoneNumber = "+456",
                MemberType = MemberType.Student
            });
        await _context.SaveChangesAsync();

        var query = new GetMembersQuery { FirstName = "alice" };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().ContainSingle(m => m.FirstName == "Alice");
    }

    [Test]
    public async Task Handle_Should_Filter_LastName_Ignoring_Case()
    {
        // Arrange
        _context.Members.AddRange(
            new Member
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice@example.com",
                PhoneNumber = "+123",
                MemberType = MemberType.Student
            },
            new Member
            {
                Id = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "Jones",
                Email = "bob@example.com",
                PhoneNumber = "+456",
                MemberType = MemberType.Student
            });
        await _context.SaveChangesAsync();

        var query = new GetMembersQuery { LastName = "sMiTh" };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().ContainSingle(m => m.LastName == "Smith");
    }

    [Test]
    public async Task Handle_Should_Return_All_Members_When_No_Filters()
    {
        // Arrange
        _context.Members.AddRange(
            new Member
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice@example.com",
                PhoneNumber = "+123",
                MemberType = MemberType.Student
            },
            new Member
            {
                Id = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "Jones",
                Email = "bob@example.com",
                PhoneNumber = "+456",
                MemberType = MemberType.Student
            });
        await _context.SaveChangesAsync();

        var query = new GetMembersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
    }
}
