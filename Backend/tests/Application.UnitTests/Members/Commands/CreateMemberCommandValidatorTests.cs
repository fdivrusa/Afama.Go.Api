using Afama.Go.Api.Application.Members.Commands;
using Afama.Go.Api.Domain.Entities;
using Afama.Go.Api.Domain.Enums;
using Afama.Go.Api.Infrastructure.Data;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Afama.Go.Api.Application.UnitTests.Members.Commands;

[TestFixture]
public class CreateMemberCommandValidatorTests
{
    private ApplicationDbContext _db;
    private CreateMemberCommandValidator _validator;

    [SetUp]
    public async Task SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"MembersDb_{Guid.NewGuid()}")
            .Options;

        _db = new ApplicationDbContext(options);
        _db.Database.EnsureCreated();
        _db.Members.Add(new Member
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            PhoneNumber = "+1111111111",
            MemberType = MemberType.Student
        });
        await _db.SaveChangesAsync();

        _validator = new CreateMemberCommandValidator(_db);
    }

    [TearDown]
    public void TearDown()
    {
        _db.Database.EnsureDeleted();
        _db.Dispose();
    }

    [Test]
    public async Task Should_Have_Error_When_FirstName_Is_Empty()
    {
        // Arrange
        var command = new CreateMemberCommand { FirstName = string.Empty };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Test]
    public async Task Should_Have_Error_When_FirstName_Is_Null()
    {
        // Arrange
        var command = new CreateMemberCommand { FirstName = null! };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Test]
    public async Task Should_Have_Error_When_FirstName_Exceeds_Maximum_Length()
    {
        // Arrange
        var command = new CreateMemberCommand { FirstName = new string('a', 65) };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_FirstName_Is_Valid()
    {
        // Arrange
        var command = new CreateMemberCommand { FirstName = "John" };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
    }

    [Test]
    public async Task Should_Have_Error_When_LastName_Is_Empty()
    {
        // Arrange
        var command = new CreateMemberCommand { LastName = string.Empty };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Test]
    public async Task Should_Have_Error_When_LastName_Is_Null()
    {
        // Arrange
        var command = new CreateMemberCommand { LastName = null! };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Test]
    public async Task Should_Have_Error_When_LastName_Exceeds_Maximum_Length()
    {
        // Arrange
        var command = new CreateMemberCommand { LastName = new string('a', 65) };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_LastName_Is_Valid()
    {
        // Arrange
        var command = new CreateMemberCommand { LastName = "Doe" };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.LastName);
    }

    [Test]
    public async Task Should_Have_Error_When_Email_Is_Empty()
    {
        // Arrange
        var command = new CreateMemberCommand { Email = string.Empty };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public async Task Should_Have_Error_When_Email_Is_Null()
    {
        // Arrange
        var command = new CreateMemberCommand { Email = null! };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public async Task Should_Have_Error_When_Email_Is_Invalid_Format()
    {
        // Arrange
        var command = new CreateMemberCommand { Email = "invalid-email" };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public async Task Should_Have_Error_When_Email_Exceeds_Maximum_Length()
    {
        // Arrange
        var longEmail = new string('a', 250) + "@test.com";
        var command = new CreateMemberCommand { Email = longEmail };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_Email_Is_Valid()
    {
        // Arrange
        var command = new CreateMemberCommand { Email = "john.doe@example.com" };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public async Task Should_Have_Error_When_PhoneNumber_Is_Empty()
    {
        // Arrange
        var command = new CreateMemberCommand { PhoneNumber = string.Empty };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Test]
    public async Task Should_Have_Error_When_PhoneNumber_Is_Null()
    {
        // Arrange
        var command = new CreateMemberCommand { PhoneNumber = null! };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Test]
    public async Task Should_Have_Error_When_PhoneNumber_Exceeds_Maximum_Length()
    {
        // Arrange
        var command = new CreateMemberCommand { PhoneNumber = new string('1', 49) };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_PhoneNumber_Is_Valid()
    {
        // Arrange
        var command = new CreateMemberCommand { PhoneNumber = "+1234567890" };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Test]
    public async Task Should_Have_Error_When_MemberType_Is_Invalid()
    {
        // Arrange
        var command = new CreateMemberCommand { MemberType = (MemberType)999 };

        // Act & Assert
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
        // Arrange
        var command = new CreateMemberCommand { MemberType = memberType };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.MemberType);
    }

    [Test]
    public async Task Should_Have_Error_When_KnownPathologies_Exceeds_Maximum_Length()
    {
        // Arrange
        var command = new CreateMemberCommand { KnownPathologies = new string('a', 2049) };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.KnownPathologies);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_KnownPathologies_Is_Valid_Length()
    {
        // Arrange
        var command = new CreateMemberCommand { KnownPathologies = new string('a', 2048) };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.KnownPathologies);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_KnownPathologies_Is_Null()
    {
        // Arrange
        var command = new CreateMemberCommand { KnownPathologies = null };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.KnownPathologies);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_KnownPathologies_Is_Empty()
    {
        // Arrange
        var command = new CreateMemberCommand { KnownPathologies = string.Empty };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.KnownPathologies);
    }

    [Test]
    public async Task Should_Have_Error_When_BirthDate_Is_In_Future()
    {
        // Arrange
        var command = new CreateMemberCommand { BirthDate = DateTime.UtcNow.AddDays(1) };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldHaveValidationErrorFor(x => x.BirthDate);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_BirthDate_Is_In_Past()
    {
        // Arrange
        var command = new CreateMemberCommand { BirthDate = DateTime.UtcNow.AddYears(-25) };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.BirthDate);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_BirthDate_Is_Null()
    {
        // Arrange
        var command = new CreateMemberCommand { BirthDate = null };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveValidationErrorFor(x => x.BirthDate);
    }

    [Test]
    public async Task Should_Pass_Validation_When_All_Properties_Are_Valid()
    {
        // Arrange
        var command = new CreateMemberCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "+1234567890",
            MemberType = MemberType.Student,
            BirthDate = DateTime.UtcNow.AddYears(-25),
            KnownPathologies = "None"
        };

        // Act & Assert
        var result = await _validator.TestValidateAsync(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public async Task Should_Have_Error_When_Email_Already_Exists()
    {
        var cmd = new CreateMemberCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            PhoneNumber = "+1234567890",
            MemberType = MemberType.Student
        };

        var result = await _validator.TestValidateAsync(cmd);

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public async Task Should_Not_Have_Error_When_Email_Is_Unique()
    {
        var cmd = new CreateMemberCommand
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.unique@example.com",
            PhoneNumber = "+1234567890",
            MemberType = MemberType.Student
        };

        var result = await _validator.TestValidateAsync(cmd);

        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
}
