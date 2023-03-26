using MassTransit;
using MediatR;
using Scribble.Content.Contracts.Events.Entities;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.Exceptions;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Models;

namespace Scribble.Content.Web.Features.Commands.Posts;

public class PublishPostCommand : IRequest
{
    public PublishPostCommand(Guid postId) => PostId = postId;
    public Guid PostId { get; }
}

public class PublishPostCommandHandler : IRequestHandler<PublishPostCommand>
{
    private readonly ILogger<PublishPostCommand> _logger;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishPostCommandHandler(ILogger<PublishPostCommand> logger, 
        IUnitOfWork<ApplicationDbContext> unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Unit> Handle(PublishPostCommand request, CancellationToken token)
    {
        var repository = _unitOfWork.GetRepository<PostEntity, Guid>();

        var post = await repository.FindAsync(new[] { request.PostId }, token)
            .ConfigureAwait(false);

        if (post is null)
            throw new EntityNotFoundException(typeof(PostEntity));

        post.IsPosted = true;

        repository.Update(post);

        await _publishEndpoint.Publish(new PostPublishedContract { PostId = post.Id }, token)
            .ConfigureAwait(false);

        return Unit.Value;
    }
}