using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.Exceptions;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Features.Commands;

// ReSharper disable once ClassNeverInstantiated.Global
public class RemoveEntityCommand<TEntity, TKey> : IRequest
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
{
    public RemoveEntityCommand(TKey key)
        => Key = key;
    public TKey Key { get; }
}

public class RemoveEntityCommandHandler<TEntity, TKey> : IRequestHandler<RemoveEntityCommand<TEntity, TKey>> 
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
{
    private readonly ILogger<RemoveEntityCommandHandler<TEntity, TKey>> _logger;
    private readonly IUnitOfWork<ApplicationDbContext> _context;

    public RemoveEntityCommandHandler(ILogger<RemoveEntityCommandHandler<TEntity, TKey>> logger, 
        IUnitOfWork<ApplicationDbContext> context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Unit> Handle(RemoveEntityCommand<TEntity, TKey> request, CancellationToken token)
    {
        var repository = _context.GetRepository<TEntity, TKey>();

        var entity = await repository.FindAsync(new[] { request.Key }, token)
            .ConfigureAwait(false);

        if (entity is null)
            throw new EntityNotFoundException(typeof(Entity));
        
        repository.Remove(entity);

        return Unit.Value;
    }
}