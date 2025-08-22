using Afama.Go.Api.Application.Common.Interfaces;
using Afama.Go.Api.Domain.Entities;

namespace Afama.Go.Api.Application.Members.Commands;

public record DeleteMemberCommand(Guid Id) : IRequest;

public class DeleteMemberCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteMemberCommand>
{
    public async Task Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await context.Members.FindAsync(new object?[] { request.Id }, cancellationToken);
        if (member == null)
        {
            throw new NotFoundException(nameof(Member), request.Id.ToString());
        }

        context.Members.Remove(member);
        await context.SaveChangesAsync(cancellationToken);
    }
}
