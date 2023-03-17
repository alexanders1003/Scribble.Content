using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Models;

namespace Scribble.Content.Web.Features.Queries;

// ReSharper disable once ClassNeverInstantiated.Global
public class GetAllCategoriesByArticleIdQuery : IRequest<ICollection<CategoryEntity>>
{
    public GetAllCategoriesByArticleIdQuery(Guid articleId) => ArticleId = articleId;
    public Guid ArticleId { get; }
}

public class GetAllCategoriesByArticleIdQueryHandler : IRequestHandler<GetAllCategoriesByArticleIdQuery, ICollection<CategoryEntity>>
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public GetAllCategoriesByArticleIdQueryHandler(IUnitOfWork<ApplicationDbContext> unitOfWork) 
        => _unitOfWork = unitOfWork;

    public async Task<ICollection<CategoryEntity>> Handle(GetAllCategoriesByArticleIdQuery request, CancellationToken token)
    {
        var repository = _unitOfWork.CreateRepository<CategoryEntity, Guid>();

        return await repository.GetAllAsync(x => x.Articles.Any(i => i.Id == request.ArticleId),
                token: token)
            .ConfigureAwait(false);
    }
}
