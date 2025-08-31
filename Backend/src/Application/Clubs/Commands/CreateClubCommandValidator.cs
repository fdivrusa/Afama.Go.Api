using Afama.Go.Api.Application.Common.Interfaces;

namespace Afama.Go.Api.Application.Clubs.Commands;
public class CreateClubCommandValidator : AbstractValidator<CreateClubCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateClubCommandValidator(IApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage(string.Format(Translations.IsRequiredMessage, nameof(CreateClubCommand.Name)))
            .MaximumLength(128).WithMessage(string.Format(Translations.MustNotExceedCharactersMessage, nameof(CreateClubCommand.Name), 128))
            .MustAsync(BeUniqueName).WithMessage(string.Format(Translations.AlreadyExistsMessage, nameof(CreateClubCommand.Name)));
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return !await _context.Clubs.AnyAsync(c => c.Name == name, cancellationToken);
    }
}
