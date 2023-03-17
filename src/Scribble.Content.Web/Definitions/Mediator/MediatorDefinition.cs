using System.Reflection;
using Calabonga.AspNetCore.AppDefinitions;
using MediatR;
using Scribble.Content.Models;
using Scribble.Content.Web.Features.Commands;
using Scribble.Content.Web.Features.Queries;
using Scribble.Shared.Models;

namespace Scribble.Content.Web.Definitions.Mediator;

public class MediatorDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddMediatR(typeof(Program).Assembly);

        services.AddScoped(typeof(IRequestHandler<GetAllEntitiesQuery<BlogEntity>, ICollection<BlogEntity>>),
            typeof(GetAllEntitiesQueryHandler<BlogEntity>));
        services.AddScoped(typeof(IRequestHandler<GetAllEntitiesQuery<ArticleEntity>, ICollection<ArticleEntity>>),
            typeof(GetAllEntitiesQueryHandler<ArticleEntity>));
        services.AddScoped(typeof(IRequestHandler<GetAllEntitiesQuery<CategoryEntity>, ICollection<CategoryEntity>>),
            typeof(GetAllEntitiesQueryHandler<CategoryEntity>));
        services.AddScoped(typeof(IRequestHandler<GetAllEntitiesQuery<TagEntity>, ICollection<TagEntity>>),
            typeof(GetAllEntitiesQueryHandler<TagEntity>));
        services.AddScoped(typeof(IRequestHandler<GetAllEntitiesQuery<CommentEntity>, ICollection<CommentEntity>>),
            typeof(GetAllEntitiesQueryHandler<CommentEntity>));
        
        services.AddScoped(typeof(IRequestHandler<CreateEntityCommand<BlogEntity>, BlogEntity>),
            typeof(CreateEntityCommandHandler<BlogEntity>));
        services.AddScoped(typeof(IRequestHandler<CreateEntityCommand<ArticleEntity>, ArticleEntity>),
            typeof(CreateEntityCommandHandler<ArticleEntity>));
        services.AddScoped(typeof(IRequestHandler<CreateEntityCommand<CategoryEntity>, CategoryEntity>),
            typeof(CreateEntityCommandHandler<CategoryEntity>));
        services.AddScoped(typeof(IRequestHandler<CreateEntityCommand<TagEntity>, TagEntity>),
            typeof(CreateEntityCommandHandler<TagEntity>));
        services.AddScoped(typeof(IRequestHandler<CreateEntityCommand<CommentEntity>, CommentEntity>),
            typeof(CreateEntityCommandHandler<CommentEntity>));
    }
}