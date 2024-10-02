using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Domain.Blogs.Events;

internal sealed record BlogCreatedEvent(Blog Blog) : IDomainEvent;