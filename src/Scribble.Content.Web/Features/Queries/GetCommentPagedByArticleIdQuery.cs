using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Content.Models;

namespace Scribble.Content.Web.Features.Queries;

public class GetCommentPagedByArticleIdQuery : IRequest<IPagedCollection<CommentEntity>>
{
    public GetCommentPagedByArticleIdQuery(Guid articleId, PaginationQueryParameters parameters)
    {
        ArticleId = articleId;
        Parameters = parameters;
    }

    public Guid ArticleId { get; }
    public PaginationQueryParameters Parameters { get; }
}

public class GetCommentPagedByArticleIdQueryHandler : IRequestHandler<GetCommentPagedByArticleIdQuery, IPagedCollection<CommentEntity>>
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public GetCommentPagedByArticleIdQueryHandler(IUnitOfWork<ApplicationDbContext> unitOfWork) 
        => _unitOfWork = unitOfWork;

    public async Task<IPagedCollection<CommentEntity>> Handle(GetCommentPagedByArticleIdQuery request,
        CancellationToken token)
    {
        var repository = _unitOfWork.CreateRepository<CommentEntity, Guid>();

        return await repository.GetPagedCollectionAsync(request.Parameters, x => x.Article.Id == request.ArticleId,
                token: token)
            .ConfigureAwait(false);
    }
}