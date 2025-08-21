using Afama.Go.Api.Application.Members.Commands;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Afama.Go.Api.Application.UnitTests.Members.Commands;

[TestFixture]
public class DeleteMemberCommandValidatorTests
{
    private DeleteMemberCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new DeleteMemberCommandValidator();
    }

    [Test]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new DeleteMemberCommand(Guid.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Should_Not_Have_Error_When_Id_Is_Provided()
    {
        var command = new DeleteMemberCommand(Guid.NewGuid());
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
