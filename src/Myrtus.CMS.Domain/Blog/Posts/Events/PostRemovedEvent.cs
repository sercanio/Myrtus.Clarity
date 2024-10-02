using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Domain.Blogs.Posts.Events;

internal sealed record PostRemovedEvent(Post Post) : IDomainEvent;