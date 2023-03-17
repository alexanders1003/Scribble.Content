using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Features.Queries;

// ReSharper disable once ClassNeverInstantiated.Global
public class GetAllEntitiesQuery<TEntity> : IRequest<ICollection<TEntity>>
    where TEntity : Entity { }

public class GetAllEntitiesQueryHandler<TEntity> : IRequestHandler<GetAllEntitiesQuery<TEntity>, ICollection<TEntity>>
    where TEntity : Entity
{
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public GetAllEntitiesQueryHandler(IUnitOfWork<ApplicationDbContext> unitOfWork) 
        => _unitOfWork = unitOfWork;

    public async Task<ICollection<TEntity>> Handle(GetAllEntitiesQuery<TEntity> request, CancellationToken token)
    {
        var repository = _unitOfWork.CreateRepository<TEntity, Guid>();

        return await repository.GetAllAsync(token: token)
            .ConfigureAwait(false);
    }
}