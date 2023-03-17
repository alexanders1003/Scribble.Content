namespace Scribble.Content.Infrastructure.UnitOfWork.Pagination;

public class PaginationQueryParameters
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; } = 20;
    public int IndexFrom { get; set; } = 0;
}