using Afama.Go.Api.Application.Common.Interfaces;

namespace Afama.Go.Api.Application.Clubs.Commands;
public class CreateClubCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;
}

public class CreateClubCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateClubCommand, Guid>
{
    public async Task<Guid> Handle(CreateClubCommand request, CancellationToken cancellationToken)
    {
        var club = new Domain.Entities.Club
        {
            Name = request.Name
        };

        context.Clubs.Add(club);
        await context.SaveChangesAsync(cancellationToken);
        return club.Id;
    }
}
