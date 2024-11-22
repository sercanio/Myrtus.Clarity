using System.Collections.Immutable;

namespace Myrtus.Clarity.Core.Domain.Abstractions;

public abstract class Entity
{
    public Guid Id { get; init; }
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? UpdatedOnUtc { get; private set; }
    public DateTime? DeletedOnUtc { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = new();

    protected Entity(Guid id)
    {
        Id = id;
        CreatedBy = "System";
        CreatedOnUtc = DateTime.UtcNow;
    }

    protected Entity()
    {
        CreatedBy = "System";
        CreatedOnUtc = DateTime.UtcNow;
    }

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
