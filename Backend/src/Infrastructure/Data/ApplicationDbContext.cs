using System.Reflection;
using Afama.Go.Api.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Afama.Go.Api.Infrastructure.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
