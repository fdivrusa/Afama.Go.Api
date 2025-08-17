using Afama.Go.Api.Application.Common.Interfaces;
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
}

public class CreateMemberCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateMemberCommand, Guid>
{
    public async Task<Guid> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = new Member
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            MemberType = request.MemberType,
            BirthDate = request.BirthDate,
            KnownPathologies = request.KnownPathologies
        };
        context.Members.Add(member);
        await context.SaveChangesAsync(cancellationToken);
        return member.Id;
    }
}
