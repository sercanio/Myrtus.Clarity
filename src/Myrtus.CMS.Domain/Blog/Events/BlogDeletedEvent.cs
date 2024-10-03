﻿using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Domain.Blogs.Events;

internal sealed record BlogDeletedEvent(Blogs.Blog Blog) : IDomainEvent;