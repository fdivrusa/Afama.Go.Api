using Afama.Go.Api.Domain.Entities;

namespace Afama.Go.Api.Application.Common.Interfaces;
public interface IApplicationDbContext
{
    DbSet<Member> Members { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
