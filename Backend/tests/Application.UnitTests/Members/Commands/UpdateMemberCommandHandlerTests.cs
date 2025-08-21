using Afama.Go.Api.Application.Members.Commands;
using Afama.Go.Api.Domain.Entities;
using Afama.Go.Api.Domain.Enums;
using Afama.Go.Api.Infrastructure.Data;
using Ardalis.GuardClauses;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Afama.Go.Api.Application.UnitTests.Members.Commands;

[TestFixture]
public class UpdateMemberCommandHandlerTests
{
    private ApplicationDbContext _context;
    private UpdateMemberCommandHandler _handler;
    private IMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);

        var configuration = new MapperConfiguration(cfg =>
            cfg.CreateMap<UpdateMemberCommand, Member>().ForMember(d => d.Id, opt => opt.Ignore()));
        _mapper = configuration.CreateMapper();

        _handler = new UpdateMemberCommandHandler(_context, _mapper);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task Handle_Should_Update_Member_When_Found()
    {
        var member = new Member
        {
            Id = Guid.NewGuid(),
            FirstName = "Old",
            LastName = "Name",
            Email = "old@example.com",
            PhoneNumber = "+123",
            MemberType = MemberType.Student
        };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var command = new UpdateMemberCommand
        {
            Id = member.Id,
            FirstName = "New",
            LastName = "Name",
            Email = "new@example.com",
            PhoneNumber = "+456",
            MemberType = MemberType.Teacher
        };

        await _handler.Handle(command, CancellationToken.None);

        var updated = await _context.Members.FindAsync(member.Id);
        updated!.FirstName.Should().Be("New");
        updated.Email.Should().Be("new@example.com");
        updated.MemberType.Should().Be(MemberType.Teacher);
    }

    [Test]
    public async Task Handle_Should_Throw_NotFound_When_Member_Does_Not_Exist()
    {
        var command = new UpdateMemberCommand
        {
            Id = Guid.NewGuid(),
            FirstName = "Any",
            LastName = "User",
            Email = "a@b.com",
            PhoneNumber = "+1",
            MemberType = MemberType.Assistant
        };

        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
