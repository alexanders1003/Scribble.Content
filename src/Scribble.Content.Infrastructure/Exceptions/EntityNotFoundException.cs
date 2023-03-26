namespace Scribble.Content.Infrastructure.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(Type entityType) => EntityType = entityType;
    public EntityNotFoundException(Type entityType, string message) : base(message) 
        => EntityType = entityType;
    public Type EntityType { get; }
}