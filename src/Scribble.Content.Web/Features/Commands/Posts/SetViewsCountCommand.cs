using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Models;

namespace Scribble.Content.Web.Features.Commands.Posts;

// ReSharper disable once ClassNeverInstantiated.Global
public class SetViewsCountCommand : IRequest
{
    public SetViewsCountCommand(Guid postId) => PostId = postId;
    public Guid PostId { get; }
}

public class SetViewsCountCommandHandler : IRequestHandler<SetViewsCountCommand>
{
    private readonly ILogger<SetViewsCountCommandHandler> _logger;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public SetViewsCountCommandHandler(ILogger<SetViewsCountCommandHandler> logger,
        IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(SetViewsCountCommand request, CancellationToken token)
    {
        var repository = _unitOfWork.GetRepository<PostEntity, Guid>();

        var article = await repository.FindAsync(new[] { request.PostId }, token: token)
            .ConfigureAwait(false);

        if (article is null) return Unit.Value;
        
        article.ViewsCount++;
        repository.Update(article);

        return Unit.Value;
    }
}