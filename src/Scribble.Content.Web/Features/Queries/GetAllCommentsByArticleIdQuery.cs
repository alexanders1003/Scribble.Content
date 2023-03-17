using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Models;

namespace Scribble.Content.Web.Features.Queries;

// ReSharper disable once ClassNeverInstantiated.Global
public class GetAllCommentsByArticleIdQuery : IRequest<ICollection<CommentEntity>>
{
    public GetAllCommentsByArticleIdQuery(Guid articleId) => ArticleId = articleId;
    public Guid ArticleId { get; }
}

public class GetAllCommentsByArticleIdQueryHandler : IRequestHandler<GetAllCommentsByArticleIdQuery, ICollection<CommentEntity>>
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public GetAllCommentsByArticleIdQueryHandler(IUnitOfWork<ApplicationDbContext> unitOfWork) 
        => _unitOfWork = unitOfWork;
    
    public async Task<ICollection<CommentEntity>> Handle(GetAllCommentsByArticleIdQuery request, CancellationToken token)
    {
        var repository = _unitOfWork.CreateRepository<CommentEntity, Guid>();

        return await repository.GetAllAsync(x => x.Article.Id == request.ArticleId,
                token: token)
            .ConfigureAwait(false);
    }
}