using Afama.Go.Api.Application.Common.Interfaces;

namespace Afama.Go.Api.Application.Members.Commands;
public class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateMemberCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(string.Format(Translations.IsRequiredMessage, nameof(CreateMemberCommand.FirstName)))
            .MaximumLength(64).WithMessage(string.Format(Translations.MustNotExceedCharactersMessage, nameof(CreateMemberCommand.FirstName), 64));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(string.Format(Translations.IsRequiredMessage, nameof(CreateMemberCommand.LastName)))
            .MaximumLength(64).WithMessage(string.Format(Translations.MustNotExceedCharactersMessage, nameof(CreateMemberCommand.LastName), 64));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(string.Format(Translations.IsRequiredMessage, nameof(CreateMemberCommand.Email)))
            .EmailAddress().WithMessage(string.Format(Translations.EmailNotValidMessage, nameof(CreateMemberCommand.Email)))
            .MaximumLength(256).WithMessage(string.Format(Translations.MustNotExceedCharactersMessage, nameof(CreateMemberCommand.Email), 256));

        RuleFor(x => x.Email)
            .MustAsync(async (email, cancellation) => !await _context.Members.AnyAsync(m => m.Email == email, cancellation))
            .WithMessage(string.Format(Translations.AlreadyExistsMessage, nameof(CreateMemberCommand.Email)));

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(string.Format(Translations.IsRequiredMessage, nameof(CreateMemberCommand.PhoneNumber)))
            .MaximumLength(48).WithMessage(string.Format(Translations.MustNotExceedCharactersMessage, nameof(CreateMemberCommand.PhoneNumber), 48));

        RuleFor(x => x.MemberType)
            .Custom((value, context) =>
            {
                if (!Enum.IsDefined(value))
                {
                    context.AddFailure(string.Format(
                        Translations.ValueNotValidMessage,
                        value,
                        nameof(CreateMemberCommand.MemberType)
                    ));
                }
            });

        RuleFor(x => x.KnownPathologies)
            .MaximumLength(2048).WithMessage(string.Format(Translations.MustNotExceedCharactersMessage, nameof(CreateMemberCommand.KnownPathologies), 2048))
            .When(x => !string.IsNullOrEmpty(x.KnownPathologies));

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.UtcNow).When(x => x.BirthDate.HasValue)
            .WithMessage(string.Format(Translations.DateMustBeInPastMessage, nameof(CreateMemberCommand.BirthDate)));

    }
}
