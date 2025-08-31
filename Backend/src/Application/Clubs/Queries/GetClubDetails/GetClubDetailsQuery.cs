using Afama.Go.Api.Application.Common.Interfaces;
using Afama.Go.Api.Domain.Entities;

namespace Afama.Go.Api.Application.Clubs.Queries.GetClubDetails;
public record GetClubDetailsQuery : IRequest<ClubDetailsDto>
{
    public Guid ClubId { get; init; }
}

public class GetClubDetailsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetClubDetailsQuery, ClubDetailsDto>
{
    public async Task<ClubDetailsDto> Handle(GetClubDetailsQuery request, CancellationToken cancellationToken)
    {
        var club = await context.Clubs
            .Where(c => c.Id == request.ClubId)
            .Include(c => c.Members)
            .FirstOrDefaultAsync(cancellationToken);

        return club == null ? throw new NotFoundException(nameof(Club), request.ClubId.ToString()) : mapper.Map<ClubDetailsDto>(club);
    }
}
