using Afama.Go.Api.Application.Common.Interfaces;

namespace Afama.Go.Api.Application.Members.Commands;

public class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{

    private readonly IApplicationDbContext _context;

    public UpdateMemberCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(string.Format(Translations.IsRequiredMessage, nameof(UpdateMemberCommand.Id)));

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(string.Format(Translations.IsRequiredMessage, nameof(UpdateMemberCommand.FirstName)))
            .MaximumLength(64).WithMessage(string.Format(Translations.MustNotExceedCharactersMessage, nameof(UpdateMemberCommand.FirstName), 64));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(string.Format(Translations.IsRequiredMessage, nameof(UpdateMemberCommand.LastName)))
            .MaximumLength(64).WithMessage(string.Format(Translations.MustNotExceedCharactersMessage, nameof(UpdateMemberCommand.LastName), 64));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(string.Format(Translations.IsRequiredMessage, nameof(UpdateMemberCommand.Email)))
            .EmailAddress().WithMessage(string.Format(Translations.EmailNotValidMessage, nameof(UpdateMemberCommand.Email)))
            .MaximumLength(256).WithMessage(string.Format(Translations.MustNotExceedCharactersMessage, nameof(UpdateMemberCommand.Email), 256));

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(string.Format(Translations.IsRequiredMessage, nameof(UpdateMemberCommand.PhoneNumber)))
            .MaximumLength(48).WithMessage(string.Format(Translations.MustNotExceedCharactersMessage, nameof(UpdateMemberCommand.PhoneNumber), 48));

        RuleFor(x => x.MemberType)
            .Custom((value, context) =>
            {
                if (!Enum.IsDefined(value))
                {
                    context.AddFailure(string.Format(
                        Translations.ValueNotValidMessage,
                        value,
                        nameof(UpdateMemberCommand.MemberType)
                    ));
                }
            });

        RuleFor(x => x.KnownPathologies)
            .MaximumLength(2048).WithMessage(string.Format(Translations.MustNotExceedCharactersMessage, nameof(UpdateMemberCommand.KnownPathologies), 2048))
            .When(x => !string.IsNullOrEmpty(x.KnownPathologies));

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.UtcNow).When(x => x.BirthDate.HasValue)
            .WithMessage(string.Format(Translations.DateMustBeInPastMessage, nameof(UpdateMemberCommand.BirthDate)));

        RuleFor(x => x)
            .MustAsync(async (command, cancellation) =>
            !await _context.Members.AnyAsync(m => m.Email == command.Email && m.Id != command.Id, cancellation));
    }
}
