using FluentValidation;
using MediatR;
using Scribble.Content.Infrastructure.Contexts;
using Scribble.Content.Infrastructure.UnitOfWork;
using Scribble.Content.Infrastructure.UnitOfWork.Pagination;
using Scribble.Content.Models;

namespace Scribble.Content.Web.Features.Queries;

// ReSharper disable once ClassNeverInstantiated.Global
public class GetBlogPagedByAuthorIdQuery : IRequest<IPagedCollection<BlogEntity>>
{
    public GetBlogPagedByAuthorIdQuery(Guid authorId, PaginationQueryParameters parameters)
    {
        AuthorId = authorId;
        Parameters = parameters;
    }

    public Guid AuthorId { get; }
    public PaginationQueryParameters Parameters { get; }
}

public class GetBlogPagedByAuthorIdQueryHandler : IRequestHandler<GetBlogPagedByAuthorIdQuery, IPagedCollection<BlogEntity>> 
{
    private readonly IUnitOfWork<ApplicationDbContext> _context;

    public GetBlogPagedByAuthorIdQueryHandler(IUnitOfWork<ApplicationDbContext> context) 
        => _context = context;
    
    public async Task<IPagedCollection<BlogEntity>> Handle(GetBlogPagedByAuthorIdQuery request, CancellationToken token)
    {
        var repository = _context.CreateRepository<BlogEntity, Guid>();
        
        return await repository.GetPagedCollectionAsync(request.Parameters, x => x.AuthorId == request.AuthorId, 
                token: token)
            .ConfigureAwait(false);
    }
}

public class GetBlogPagedByAuthorIdQueryValidator : AbstractValidator<GetBlogPagedByAuthorIdQuery>
{
    public GetBlogPagedByAuthorIdQueryValidator()
    {
        RuleFor(x => x.AuthorId)
            .NotEqual(Guid.Empty);
        RuleFor(x => x.Parameters.PageIndex)
            .GreaterThan(0);
        RuleFor(x => x.Parameters.PageSize)
            .GreaterThan(0);
        RuleFor(x => x.Parameters.IndexFrom)
            .GreaterThanOrEqualTo(0);
    }
}