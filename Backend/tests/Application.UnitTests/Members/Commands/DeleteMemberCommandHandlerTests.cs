using Afama.Go.Api.Application.Members.Commands;
using Afama.Go.Api.Domain.Entities;
using Afama.Go.Api.Domain.Enums;
using Afama.Go.Api.Infrastructure.Data;
using Ardalis.GuardClauses;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Afama.Go.Api.Application.UnitTests.Members.Commands;

[TestFixture]
public class DeleteMemberCommandHandlerTests
{
    private ApplicationDbContext _context;
    private DeleteMemberCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
        _handler = new DeleteMemberCommandHandler(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task Handle_Should_Delete_Member_When_Found()
    {
        var member = new Member
        {
            Id = Guid.NewGuid(),
            FirstName = "Delete",
            LastName = "Me",
            Email = "delete@example.com",
            PhoneNumber = "+789",
            MemberType = MemberType.Student
        };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var command = new DeleteMemberCommand(member.Id);
        await _handler.Handle(command, CancellationToken.None);

        var deleted = await _context.Members.FindAsync(member.Id);
        deleted.Should().BeNull();
    }

    [Test]
    public async Task Handle_Should_Throw_NotFound_When_Member_Not_Exist()
    {
        var command = new DeleteMemberCommand(Guid.NewGuid());
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
