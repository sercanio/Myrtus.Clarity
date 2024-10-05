using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Domain.Blogs.Events;

namespace Myrtus.CMS.Domain.Blogs.Events;

internal sealed record BlogDeletedEvent(Guid BlogId) : IDomainEvent;