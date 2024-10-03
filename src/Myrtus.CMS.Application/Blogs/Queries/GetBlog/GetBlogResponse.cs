﻿using Myrtus.CMS.Domain.Blogs.Common;

namespace Myrtus.CMS.Application.Blogs.Queries.GetBlog
{
    public sealed class BlogResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Slug { get; init; }
        public Guid OwnerId { get; init; }
        public DateTime CreatedOnUtc { get; init; }
        public DateTime? UpdatedOnUtc { get; init; }
        public DateTime? DeletedOnUtc { get; init; }
    }
}