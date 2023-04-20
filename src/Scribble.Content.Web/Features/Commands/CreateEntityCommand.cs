using AutoMapper;
using FluentValidation;
using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Web.Models;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Features.Commands;

// ReSharper disable once ClassNeverInstantiated.Global
public class CreateEntityCommand<TEntity, TKey, TViewModel> : IRequest<TEntity>
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    where TViewModel : ViewModel
{
    public CreateEntityCommand(TViewModel viewModel) => ViewModel = viewModel;
    public TViewModel ViewModel { get; }
}

public class CreateEntityCommandHandler<TEntity, TKey, TViewModel> : IRequestHandler<CreateEntityCommand<TEntity, TKey, TViewModel>, TEntity> 
    where TEntity : Entity<TKey> where TKey : IEquatable<TKey>
    where TViewModel : ViewModel
{
    private readonly ILogger<CreateEntityCommandHandler<TEntity, TKey, TViewModel>> _logger;
    private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
    private readonly IValidator<TViewModel> _validator;
    private readonly IMapper _mapper;

    public CreateEntityCommandHandler(ILogger<CreateEntityCommandHandler<TEntity, TKey, TViewModel>> logger, 
        IUnitOfWork<ApplicationDbContext> unitOfWork, IValidator<TViewModel> validator, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<TEntity> Handle(CreateEntityCommand<TEntity, TKey, TViewModel> request, CancellationToken token)
    {
        var validationResult = await _validator.ValidateAsync(request.ViewModel, token)
            .ConfigureAwait(false);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
        
        var repository = _unitOfWork.GetRepository<TEntity, TKey>();
        
        var entity = _mapper.Map<TEntity>(request.ViewModel);
        
        return await repository.InsertAsync(entity, token)
            .ConfigureAwait(false);
    }
}