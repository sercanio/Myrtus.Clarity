using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Domain.Blogs.Events;

public sealed record BlogCreatedEvent(Guid BlogId) : IDomainEvent;