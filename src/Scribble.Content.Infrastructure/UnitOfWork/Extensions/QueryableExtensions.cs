using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;

namespace Scribble.Content.Infrastructure.UnitOfWork.Extensions;

public static class QueryableExtensions
{
    public static async Task<IPagedCollection<TEntity>> ToPagedCollectionAsync<TEntity>(this IQueryable<TEntity> source, 
        PaginationQueryParameters parameters,
        CancellationToken token = default)
    {
        if (parameters.IndexFrom > parameters.PageIndex)
            throw new ArgumentException(
                $"indexFrom: {parameters.IndexFrom} > pageIndex: {parameters.IndexFrom}, must indexFrom <= pageIndex");

        var count = await source.CountAsync(token)
            .ConfigureAwait(false);

        var entities = await source
            .Skip((parameters.PageIndex - parameters.IndexFrom) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync(token)
            .ConfigureAwait(false);

        return new PagedCollection<TEntity>
        {
            PageIndex = parameters.PageIndex,
            PageSize = parameters.PageSize,
            IndexFrom = parameters.IndexFrom,
            TotalCount = count,
            Entities = entities,
            TotalPages = (int)Math.Ceiling(count / (double)parameters.PageSize)
        };
    }
    
    public static async Task<ICollection<TEntity>> ToCollectionAsync<TEntity>(this IQueryable<TEntity> source)
        => new Collection<TEntity>(await source.ToListAsync());
}