using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Models;

namespace Scribble.Content.Web.Features.Queries.Posts;

public class GetPendingPostsQuery : IRequest<IEnumerable<PostEntity>> { }

public class GetPendingPostsQueryHandler : IRequestHandler<GetPendingPostsQuery, IEnumerable<PostEntity>>
{
    private readonly ILogger<GetPendingPostsQuery> _logger;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public GetPendingPostsQueryHandler(ILogger<GetPendingPostsQuery> logger, 
        IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PostEntity>> Handle(GetPendingPostsQuery request, CancellationToken token)
    {
        var repository = _unitOfWork.GetRepository<PostEntity, Guid>();

        return await repository.GetAllAsync(x => !x.IsPosted, token: token)
            .ConfigureAwait(false);
    }
}