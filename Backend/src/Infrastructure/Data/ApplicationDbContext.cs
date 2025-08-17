using System.Linq.Expressions;
using System.Reflection;
using Afama.Go.Api.Application.Common.Interfaces;
using Afama.Go.Api.Domain.Common;
using Afama.Go.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Afama.Go.Api.Infrastructure.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Member> Members => Set<Member>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType) && !entityType.ClrType.IsAbstract)
            {
                builder.Entity(entityType.ClrType).HasQueryFilter(CreateIsDeletedFilter(entityType.ClrType));
            }
        }

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private static LambdaExpression CreateIsDeletedFilter(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "e");

        var isDeletedProperty = entityType.GetProperty(nameof(BaseEntity.IsDeleted));
        if (isDeletedProperty == null || isDeletedProperty.PropertyType != typeof(bool))
            return Expression.Lambda(Expression.Constant(true), parameter);

        var propertyAccess = Expression.Property(parameter, isDeletedProperty);

        var notDeleted = Expression.Equal(propertyAccess, Expression.Constant(false));

        return Expression.Lambda(notDeleted, parameter);
    }
}
