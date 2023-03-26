using Scribble.Shared.Models;

namespace Scribble.Content.Infrastructure.UnitOfWork.Factories;

public interface IEntityRepositoryFactory
{
    IEntityRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
        where TEntity : class
        where TKey : IEquatable<TKey>;
}