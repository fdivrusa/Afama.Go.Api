using System.Reflection;
using Afama.Go.Api.Application.Common.Interfaces;
using Afama.Go.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Afama.Go.Api.Infrastructure.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Member> Members => Set<Member>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
