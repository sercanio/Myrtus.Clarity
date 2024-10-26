
namespace Myrtus.Clarity.Core.Domain.Abstractions;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();
    protected Entity(Guid id)
    {
        Id = id;
    }

    protected Entity()
    {
    }

    public Guid Id { get; init; }
    public DateTime CreatedOnUtc { get; private set; }  = DateTime.UtcNow;
    public DateTime? UpdatedOnUtc { get; private set; } = null!;
    public DateTime? DeletedOnUtc { get; private set; } = null!;

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void MarkUpdated()
    {
        UpdatedOnUtc = DateTime.UtcNow;
    }

    public void MarkDeleted()
    {
        DeletedOnUtc = DateTime.UtcNow;
    }
}
