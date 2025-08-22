using Afama.Go.Api.Application.Common.Interfaces;
using Afama.Go.Api.Application.Common.Mappings;
using Afama.Go.Api.Domain.Entities;
using Afama.Go.Api.Domain.Enums;

namespace Afama.Go.Api.Application.Members.Commands;

public record UpdateMemberCommand : IRequest
{
    public Guid Id { get; init; }
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
            CreateMap<UpdateMemberCommand, Member>().IgnoreAuditableEntity()
                .ForMember(dest => dest.Clubs, opt => opt.Ignore());
        }
    }
}

public class UpdateMemberCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<UpdateMemberCommand>
{
    public async Task Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await context.Members.FindAsync(new object?[] { request.Id }, cancellationToken);
        if (member == null)
        {
            throw new NotFoundException(nameof(Member), request.Id.ToString());
        }

        mapper.Map(request, member);

        await context.SaveChangesAsync(cancellationToken);
    }
}
