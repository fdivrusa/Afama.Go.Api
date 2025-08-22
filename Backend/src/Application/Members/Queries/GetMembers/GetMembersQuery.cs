using Afama.Go.Api.Application.Common.Interfaces;
using Afama.Go.Api.Domain.Enums;

namespace Afama.Go.Api.Application.Members.Queries.GetMembers;

public record GetMembersQuery : IRequest<IEnumerable<MemberBriefDto>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public MemberType? MemberType { get; set; }
}

public class GetMembersQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetMembersQuery, IEnumerable<MemberBriefDto>>
{
    public async Task<IEnumerable<MemberBriefDto>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
    {
        var query = context.Members.AsQueryable();
        if (!string.IsNullOrEmpty(request.FirstName))
        {
            query = query.Where(m => m.FirstName != null && m.FirstName.Contains(request.FirstName, StringComparison.OrdinalIgnoreCase));
        }
        if (!string.IsNullOrEmpty(request.LastName))
        {
            query = query.Where(m => m.LastName != null && m.LastName.Contains(request.LastName, StringComparison.OrdinalIgnoreCase));
        }
        if (request.MemberType.HasValue)
        {
            query = query.Where(m => m.MemberType == request.MemberType.Value);
        }
        var members = await query.ToListAsync(cancellationToken);
        return mapper.Map<IEnumerable<MemberBriefDto>>(members);
    }
}
