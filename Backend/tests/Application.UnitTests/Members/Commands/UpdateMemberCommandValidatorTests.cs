using Afama.Go.Api.Application.Members.Commands;
using Afama.Go.Api.Domain.Enums;
using Afama.Go.Api.Infrastructure.Data;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Afama.Go.Api.Application.UnitTests.Members.Commands;

[TestFixture]
public class UpdateMemberCommandValidatorTests
{
    private ApplicationDbContext _context = null!;
    private UpdateMemberCommandValidator _validator = null!;

    [SetUp]
    public async Task SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        await _context.Database.EnsureCreatedAsync();

        _context.Members.Add(new Domain.Entities.Member
        {
            Id = Guid.NewGuid(),
            FirstName = "Existing",
            LastName = "Member",
            Email = "test@test.com",
            PhoneNumber = "+1234567890",
            MemberType = MemberType.Student
        });
        await _context.SaveChangesAsync();

        _validator = new UpdateMemberCommandValidator(_context);
    }

    [TearDown]
    public async Task TearDown()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }

    private static UpdateMemberCommand CreateValidCommand() => new()
    {
        Id = Guid.NewGuid(),
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@example.com",
        PhoneNumber = "+1234567890",
        MemberType = MemberType.Student,
        BirthDate = DateTime.UtcNow.AddYears(-20),
        KnownPathologies = "None"
    };

    [Test]
    public async Task Should_Have_Error_When_Id_Is_Empty()
    {
        var command = CreateValidCommand() with { Id = Guid.Empty };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_Id_Is_Provided()
    {
        var command = CreateValidCommand();
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public async Task Should_Have_Error_When_FirstName_Is_Empty()
    {
        var command = CreateValidCommand() with { FirstName = string.Empty };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Test]
    public async Task Should_Have_Error_When_FirstName_Is_Null()
    {
        var command = CreateValidCommand();
        command = command with { FirstName = null! };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Test]
    public async Task Should_Have_Error_When_FirstName_Exceeds_Maximum_Length()
    {
        var command = CreateValidCommand() with { FirstName = new string('a', 65) };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_FirstName_Is_Valid()
    {
        var command = CreateValidCommand();
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
    }

    [Test]
    public async Task Should_Have_Error_When_LastName_Is_Empty()
    {
        var command = CreateValidCommand() with { LastName = string.Empty };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Test]
    public async Task Should_Have_Error_When_LastName_Is_Null()
    {
        var command = CreateValidCommand();
        command = command with { LastName = null! };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Test]
    public async Task Should_Have_Error_When_LastName_Exceeds_Maximum_Length()
    {
        var command = CreateValidCommand() with { LastName = new string('a', 65) };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_LastName_Is_Valid()
    {
        var command = CreateValidCommand();
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.LastName);
    }

    [Test]
    public async Task Should_Have_Error_When_Email_Is_Empty()
    {
        var command = CreateValidCommand() with { Email = string.Empty };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public async Task Should_Have_Error_When_Email_Is_Null()
    {
        var command = CreateValidCommand();
        command = command with { Email = null! };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public async Task Should_Have_Error_When_Email_Is_Invalid()
    {
        var command = CreateValidCommand() with { Email = "invalid" };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public async Task Should_Have_Error_When_Email_Exceeds_Maximum_Length()
    {
        var longEmail = new string('a', 250) + "@test.com";
        var command = CreateValidCommand() with { Email = longEmail };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_Email_Is_Valid()
    {
        var command = CreateValidCommand();
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public async Task Should_Have_Error_When_PhoneNumber_Is_Empty()
    {
        var command = CreateValidCommand() with { PhoneNumber = string.Empty };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Test]
    public async Task Should_Have_Error_When_PhoneNumber_Is_Null()
    {
        var command = CreateValidCommand();
        command = command with { PhoneNumber = null! };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Test]
    public async Task Should_Have_Error_When_PhoneNumber_Exceeds_Maximum_Length()
    {
        var command = CreateValidCommand() with { PhoneNumber = new string('1', 49) };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_PhoneNumber_Is_Valid()
    {
        var command = CreateValidCommand();
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Test]
    public async Task Should_Have_Error_When_MemberType_Is_Invalid()
    {
        var command = CreateValidCommand() with { MemberType = (MemberType)999 };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.MemberType);
    }

    [TestCase(MemberType.Student)]
    [TestCase(MemberType.Assistant)]
    [TestCase(MemberType.Teacher)]
    [TestCase(MemberType.Parent)]
    [TestCase(MemberType.Other)]
    public async Task Should_Not_Have_Error_When_MemberType_Is_Valid(MemberType memberType)
    {
        var command = CreateValidCommand() with { MemberType = memberType };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.MemberType);
    }

    [Test]
    public async Task Should_Have_Error_When_KnownPathologies_Exceeds_Maximum_Length()
    {
        var command = CreateValidCommand() with { KnownPathologies = new string('a', 2049) };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.KnownPathologies);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_KnownPathologies_Is_Valid_Length()
    {
        var command = CreateValidCommand() with { KnownPathologies = new string('a', 2048) };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.KnownPathologies);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_KnownPathologies_Is_Null()
    {
        var command = CreateValidCommand() with { KnownPathologies = null };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.KnownPathologies);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_KnownPathologies_Is_Empty()
    {
        var command = CreateValidCommand() with { KnownPathologies = string.Empty };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.KnownPathologies);
    }

    [Test]
    public async Task Should_Have_Error_When_BirthDate_Is_In_Future()
    {
        var command = CreateValidCommand() with { BirthDate = DateTime.UtcNow.AddDays(1) };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.BirthDate);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_BirthDate_Is_In_Past()
    {
        var command = CreateValidCommand() with { BirthDate = DateTime.UtcNow.AddYears(-30) };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.BirthDate);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_BirthDate_Is_Null()
    {
        var command = CreateValidCommand() with { BirthDate = null };
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.BirthDate);
    }

    [Test]
    public async Task Should_Pass_Validation_When_All_Properties_Are_Valid()
    {
        var command = CreateValidCommand();
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
