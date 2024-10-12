﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using MediatR;
using Ardalis.Result;
using Myrtus.CMS.Application.Blogs.Queries.GetBlog;
using Myrtus.CMS.Application.Blogs.Commands.CreateBlog;
using Myrtus.CMS.Application.Blogs.Queries.GetAllBlogs;
using Myrtus.CMS.Application.Blogs.Commands.DeleteBlog;
using Myrtus.CMS.Application.Blogs.Commands.UpdateBlog;
using Myrtus.CMS.WebAPI.Controllers;
using Myrtus.Clarity.Core.WebApi;

namespace Myrtus.CMS.Api.Controllers.Blogs;

[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/blogs")]
public class BlogsController : BaseController
{
    public BlogsController(ISender sender, IErrorHandlingService errorHandlingService)
        : base(sender, errorHandlingService)
    {
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlog(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBlogQuery(id);
        Result<BlogResponse> result = await _sender.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return _errorHandlingService.HandleErrorResponse(result);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlog(CreateBlogRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateBlogCommand(
            request.Title,
        request.Slug,
        request.UserId);

        Result<CreateBlogCommandResponse> result = await _sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return _errorHandlingService.HandleErrorResponse(result);
        }

        return CreatedAtAction(nameof(GetBlog), new { id = result.Value.Id }, result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBlogs([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetAllBlogsQuery(pageIndex, pageSize);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsNotFound())
        {
           return _errorHandlingService.HandleErrorResponse(result);
        }

        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlog(Guid id, UpdateBlogRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateBlogCommand(
            id,
            request.UpdatedById,
            request.Title,
        request.Slug,
        request.Description);

        Result<UpdateBlogCommandResponse> result = await _sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return _errorHandlingService.HandleErrorResponse(result);
        }

        return CreatedAtAction(nameof(GetBlog), new { id = result.Value.Id }, result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlog(Guid id)
    {
        var command = new DeleteBlogCommand(id);
        Result<DeleteBlogCommandResponse> result = await _sender.Send(command);

        if (!result.IsSuccess)
        {
            return _errorHandlingService.HandleErrorResponse(result);
        }

        return Ok(result.Value);
    }
}
