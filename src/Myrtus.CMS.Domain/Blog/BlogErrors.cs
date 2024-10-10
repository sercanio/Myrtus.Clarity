﻿using Myrtus.Clarity.Core.Domain.Abstractions;

namespace Myrtus.CMS.Domain.Blogs;

public static class BlogErrors
{
    public static readonly Error NotFound = new(
        "Blog.NotFound",
        404,
        "The blog with the specified identifier was not found");

    public static readonly Error Overlap = new(
        "Blog.Overlap",
        409,
        "The current blog is overlapping with an existing one");
    
    public static readonly Error TitleAlreadyExists = new(
        "Blog.TitleAlreadyExists",
        409,
        "The current blog title is overlapping with an existing one");
    
    public static readonly Error SlugAlreadyExists = new(
        "Blog.SlugAlreadyExists",
        409,
        "The current blog slug is overlapping with an existing one");
    
    public static readonly Error SlugContainsInvalidCharacters= new(
        "Blog.SlugContainsInvalidCharacters",
        400,
        "The current blog slug contains invalid characters");
}
