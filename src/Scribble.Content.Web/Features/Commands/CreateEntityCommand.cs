using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Features.Commands;

// ReSharper disable once ClassNeverInstantiated.Global
public class CreateEntityCommand<TEntity> : IRequest<TEntity>
    where TEntity : Entity
{
    public CreateEntityCommand(TEntity model) => Model = model;
    public TEntity Model { get; }
}

public class CreateEntityCommandHandler<TEntity> : IRequestHandler<CreateEntityCommand<TEntity>, TEntity> 
    where TEntity : Entity
{
    private readonly ILogger<CreateEntityCommandHandler<TEntity>> _logger;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;

    public CreateEntityCommandHandler(ILogger<CreateEntityCommandHandler<TEntity>> logger, 
        IUnitOfWork<ApplicationDbContext> unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<TEntity> Handle(CreateEntityCommand<TEntity> request, CancellationToken token)
    {
        var repository = _unitOfWork.CreateRepository<TEntity, Guid>();

        return await repository.InsertAsync(request.Model, token)
            .ConfigureAwait(false);
    }
}