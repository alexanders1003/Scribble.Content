using Scribble.Content.Infrastructure.UnitOfWork.Pagination;

namespace Scribble.Content.Infrastructure.UnitOfWork.Extensions;

public static class PagedCollectionExtensions
{
    public static IPagedCollection<TEntity> Empty<TEntity>()
        => new PagedCollection<TEntity>();

    public static IPagedCollection<TEntity> From<TEntity>(IPagedCollection<TEntity> source)
        => new PagedCollection<TEntity>(source);
}