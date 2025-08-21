namespace Afama.Go.Api.Application.Members.Commands;

public class DeleteMemberCommandValidator : AbstractValidator<DeleteMemberCommand>
{
    public DeleteMemberCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(string.Format(Translations.IsRequiredMessage, nameof(DeleteMemberCommand.Id)));
    }
}
