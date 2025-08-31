using Afama.Go.Api.Application.Common.Interfaces;

namespace Afama.Go.Api.Application.Clubs.Queries.GetClubs;
public record GetClubsQuery : IRequest<IEnumerable<ClubBriefDto>>
{
    public string? Name { get; set; }
}

public class GetClubsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetClubsQuery, IEnumerable<ClubBriefDto>>
{
    public async Task<IEnumerable<ClubBriefDto>> Handle(GetClubsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Clubs.AsQueryable();
        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(c => c.Name != null && c.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
        }
        var clubs = await query.ToListAsync(cancellationToken);
        return mapper.Map<IEnumerable<ClubBriefDto>>(clubs);
    }
}
