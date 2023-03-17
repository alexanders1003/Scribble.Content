namespace Scribble.Content.Infrastructure.UnitOfWork.Pagination;

public interface IPagedCollection<TEntity>
{
    int IndexFrom { get; }
    int PageIndex { get; }
    int PageSize { get; }
    int TotalCount { get; }
    int TotalPages { get; }
    ICollection<TEntity> Entities { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
}