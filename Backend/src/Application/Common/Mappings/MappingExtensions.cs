using Afama.Go.Api.Application.Common.Models;
using Afama.Go.Api.Domain.Common;

namespace Afama.Go.Api.Application.Common.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
        => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();

    public static IMappingExpression<TSource, TDestination> IgnoreAuditableEntity<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mapping)
        where TDestination : BaseAuditableEntity
        => mapping
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.Created, opt => opt.Ignore())
            .ForMember(d => d.CreatedBy, opt => opt.Ignore())
            .ForMember(d => d.LastModified, opt => opt.Ignore())
            .ForMember(d => d.LastModifiedBy, opt => opt.Ignore())
            .ForMember(d => d.IsDeleted, opt => opt.Ignore())
            .ForMember(d => d.Deleted, opt => opt.Ignore())
            .ForMember(d => d.DeletedBy, opt => opt.Ignore())
            .ForMember(d => d.DomainEvents, opt => opt.Ignore());
}
