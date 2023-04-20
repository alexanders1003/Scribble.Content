using AutoMapper;
using FluentValidation;
using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.Exceptions;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Web.Models;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Features.Commands;

// ReSharper disable once ClassNeverInstantiated.Global
public class UpdateEntityCommand<TEntity, TKey, TViewModel> : IRequest 
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    where TViewModel : ViewModel
{
    public UpdateEntityCommand(TKey key, TViewModel viewModel)
    {
        Key = key;
        ViewModel = viewModel;
    }
    
    public TKey Key { get; }
    public TViewModel ViewModel { get; }
}

public class UpdateEntityCommandHandler<TEntity, TKey, TViewModel> : IRequestHandler<UpdateEntityCommand<TEntity, TKey, TViewModel>> 
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    where TViewModel : ViewModel
{
    private readonly ILogger<UpdateEntityCommandHandler<TEntity, TKey, TViewModel>> _logger;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
    private readonly IValidator<TViewModel> _validator;
    private readonly IMapper _mapper;

    public UpdateEntityCommandHandler(ILogger<UpdateEntityCommandHandler<TEntity, TKey, TViewModel>> logger, 
        IUnitOfWork<ApplicationDbContext> unitOfWork, IValidator<TViewModel> validator, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _mapper = mapper;
    }
    
    public async Task<Unit> Handle(UpdateEntityCommand<TEntity, TKey, TViewModel> request, CancellationToken token)
    {
        var validationResult = await _validator.ValidateAsync(request.ViewModel, token)
            .ConfigureAwait(false);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        var repository = _unitOfWork.GetRepository<TEntity, TKey>();

        var entity = await repository.FindAsync(new[] { request.Key }, token);

        if (entity is null)
            throw new EntityNotFoundException(typeof(TEntity));

        var updatedEntity = _mapper.Map(request.ViewModel, entity);
        
        repository.Update(updatedEntity);

        return Unit.Value;
    }
}