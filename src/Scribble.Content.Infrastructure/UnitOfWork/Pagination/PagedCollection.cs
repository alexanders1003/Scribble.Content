using System.Collections.ObjectModel;

namespace Scribble.Content.Infrastructure.UnitOfWork.Pagination;

public class PagedCollection<TEntity> : IPagedCollection<TEntity>
{
    public int IndexFrom { get; init; }
    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public ICollection<TEntity> Entities { get; init; } = null!;
    public bool HasPreviousPage => PageIndex - IndexFrom > 0;
    public bool HasNextPage => PageIndex - IndexFrom + 1 < TotalPages;

    private PagedCollection(PaginationQueryParameters parameters)
    {
        if (parameters.IndexFrom > parameters.PageIndex)
            throw new ArgumentException(
                $"indexFrom: {parameters.IndexFrom} > pageIndex: {parameters.PageIndex}, must indexFrom <= pageIndex");

        if (parameters.PageSize is < 0 or 0)
            throw new ArgumentException($"pageSize: {parameters.PageSize} cannot be less than zero!", nameof(parameters.PageSize));
    }

    public PagedCollection(IEnumerable<TEntity> source, PaginationQueryParameters parameters)
        : this(parameters)
    {
        var enumerable = source as TEntity[] ?? source.ToArray();

        PageIndex = parameters.PageIndex;
        PageSize = parameters.PageSize;
        IndexFrom = parameters.IndexFrom;
        TotalCount = enumerable.Length;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

        var items = enumerable
            .Skip((PageIndex - IndexFrom) * PageSize)
            .Take(PageSize)
            .ToArray();

        Entities = new Collection<TEntity>(items);
    }

    public PagedCollection(IQueryable<TEntity> source, PaginationQueryParameters parameters)
        : this(parameters)
    {
        PageIndex = parameters.PageIndex;
        PageSize = parameters.PageSize;
        IndexFrom = parameters.IndexFrom;
        TotalCount = source.Count();
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

        var items = source
            .Skip((PageIndex - IndexFrom) * PageSize)
            .Take(PageSize)
            .ToArray();

        Entities = new Collection<TEntity>(items);
    }

    public PagedCollection(IPagedCollection<TEntity> source)
    {
        PageIndex = source.PageIndex;
        PageSize = source.PageSize;
        IndexFrom = source.IndexFrom;
        TotalCount = source.TotalCount;
        TotalPages = source.TotalPages;
        
        Entities = source.Entities;
    }
    
    internal PagedCollection() => Entities = Array.Empty<TEntity>();
}
