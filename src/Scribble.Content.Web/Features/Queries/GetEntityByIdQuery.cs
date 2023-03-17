using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Features.Queries;

// ReSharper disable once ClassNeverInstantiated.Global
public class GetEntityByIdQuery<TEntity> : IRequest<TEntity?>
    where TEntity : Entity
{
    public GetEntityByIdQuery(Guid id) => Id = id;
    public Guid Id { get; }
}

public class GetEntityByIdQueryHander<TEntity> : IRequestHandler<GetEntityByIdQuery<TEntity>, TEntity?> 
    where TEntity : Entity
{
    private readonly IUnitOfWork<ApplicationDbContext> _context;

    public GetEntityByIdQueryHander(IUnitOfWork<ApplicationDbContext> context) 
        => _context = context;
    
    public async Task<TEntity?> Handle(GetEntityByIdQuery<TEntity> request, CancellationToken token)
    {
        var repository = _context.CreateRepository<TEntity, Guid>();

        return await repository.GetFirstOrDefaultAsync(x => x.Id == request.Id, token: token)
            .ConfigureAwait(false);
    }
}