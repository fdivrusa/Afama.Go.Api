using Afama.Go.Api.Application.Common.Interfaces;
using Afama.Go.Api.Application.Common.Mappings;
using Afama.Go.Api.Domain.Entities;
using Afama.Go.Api.Domain.Enums;

namespace Afama.Go.Api.Application.Members.Commands;
public record CreateMemberCommand : IRequest<Guid>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public MemberType MemberType { get; init; } = MemberType.Other;
    public DateTime? BirthDate { get; init; } = null;
    public string? KnownPathologies { get; init; } = null;
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateMemberCommand, Member>().IgnoreAuditableEntity();
        }
    }
}

public class CreateMemberCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<CreateMemberCommand, Guid>
{
    public async Task<Guid> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = mapper.Map<Member>(request);
        context.Members.Add(member);
        await context.SaveChangesAsync(cancellationToken);
        return member.Id;
    }
}
