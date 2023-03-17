using System.Collections.ObjectModel;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;

namespace Scribble.Content.Infrastructure.UnitOfWork.Extensions;

public static class EnumerableExtensions
{
    public static IPagedCollection<TEntity> ToPagedCollection<TEntity>(this IEnumerable<TEntity> source, PaginationQueryParameters parameters)
        => new PagedCollection<TEntity>(source, parameters);

    public static ICollection<TEntity> ToCollection<TEntity>(this IEnumerable<TEntity> source)
        => new Collection<TEntity>(source.ToList());
}