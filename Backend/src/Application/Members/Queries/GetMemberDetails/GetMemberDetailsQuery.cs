using Afama.Go.Api.Application.Common.Interfaces;
using Afama.Go.Api.Domain.Entities;

namespace Afama.Go.Api.Application.Members.Queries.GetMemberDetails;
public record GetMemberDetailsQuery : IRequest<MemberDetailsDto>
{
    public Guid MemberId { get; init; }
}

public class GetMemberDetailsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetMemberDetailsQuery, MemberDetailsDto>
{
    public async Task<MemberDetailsDto> Handle(GetMemberDetailsQuery request, CancellationToken cancellationToken)
    {
        var member = await context.Members
            .Where(m => m.Id == request.MemberId)
            .FirstOrDefaultAsync(cancellationToken);

        return member == null ? throw new NotFoundException(nameof(Member), request.MemberId.ToString()) : mapper.Map<MemberDetailsDto>(member);
    }
}
