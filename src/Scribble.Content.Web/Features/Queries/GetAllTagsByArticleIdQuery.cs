using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Models;

namespace Scribble.Content.Web.Features.Queries;

// ReSharper disable once ClassNeverInstantiated.Global
public class GetAllTagsByArticleIdQuery : IRequest<ICollection<TagEntity>>
{
    public GetAllTagsByArticleIdQuery(Guid articleId) => ArticleId = articleId;
    public Guid ArticleId { get; }
}

public class GetAllTagsByArticleIdQueryHandler : IRequestHandler<GetAllTagsByArticleIdQuery, ICollection<TagEntity>>
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public GetAllTagsByArticleIdQueryHandler(IUnitOfWork<ApplicationDbContext> unitOfWork) 
        => _unitOfWork = unitOfWork;
    
    public async Task<ICollection<TagEntity>> Handle(GetAllTagsByArticleIdQuery request, CancellationToken token)
    {
        var repository = _unitOfWork.CreateRepository<TagEntity, Guid>();

        return await repository.GetAllAsync(x => x.Articles.Any(i => i.Id == request.ArticleId),
                token: token)
            .ConfigureAwait(false);
    }
}