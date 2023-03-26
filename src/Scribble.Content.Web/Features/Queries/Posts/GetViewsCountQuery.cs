using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Models;

namespace Scribble.Content.Web.Features.Queries.Posts;

// ReSharper disable once ClassNeverInstantiated.Global
public class GetViewsCountQuery : IRequest<int>
{
    public GetViewsCountQuery(Guid articleId) => ArticleId = articleId;
    public Guid ArticleId { get; }
}

public class GetViewsCountQueryHandler : IRequestHandler<GetViewsCountQuery, int>
{
    private readonly ILogger<GetViewsCountQueryHandler> _logger;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public GetViewsCountQueryHandler(ILogger<GetViewsCountQueryHandler> logger, IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(GetViewsCountQuery request, CancellationToken token)
    {
        var repository = _unitOfWork.GetRepository<PostEntity, Guid>();

        var article = await repository.FindAsync(new[] { request.ArticleId }, token: token)
            .ConfigureAwait(false);

        return article?.ViewsCount ?? 0;
    }
}