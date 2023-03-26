using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Features.Queries;

// ReSharper disable once ClassNeverInstantiated.Global
public class GetEntityByIdQuery<TEntity, TKey> : IRequest<TEntity?>
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
{
    public GetEntityByIdQuery(TKey id) => Id = id;
    public TKey Id { get; }
}

public class GetEntityByIdQueryHandler<TEntity, TKey> : IRequestHandler<GetEntityByIdQuery<TEntity, TKey>, TEntity?> 
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
{
    private readonly ILogger<GetEntityByIdQueryHandler<TEntity, TKey>> _logger;
    private readonly IUnitOfWork<ApplicationDbContext> _context;

    public GetEntityByIdQueryHandler(ILogger<GetEntityByIdQueryHandler<TEntity, TKey>> logger, 
        IUnitOfWork<ApplicationDbContext> context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<TEntity?> Handle(GetEntityByIdQuery<TEntity, TKey> request, CancellationToken token)
    {
        var repository = _context.GetRepository<TEntity, TKey>();

        return await repository.GetFirstOrDefaultAsync(x => x.Id.Equals(request.Id), token: token)
            .ConfigureAwait(false);
    }
}