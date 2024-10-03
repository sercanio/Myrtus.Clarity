﻿using Asp.Versioning;
using Myrtus.CMS.WebAPI.Controllers;
using MediatR;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Myrtus.CMS.Application.Blogs.Queries.GetBlog;
using Myrtus.CMS.Application.Blogs.Commands.CreateBlog;
using Myrtus.CMS.Application.Blogs.Queries.GetAllBlogs;
using Myrtus.CMS.Application.Blogs.Commands.DeleteBlog;
using Myrtus.CMS.Application.Blogs.Commands.UpdateBlog;

namespace Myrtus.CMS.Api.Controllers.Blogs;

[Authorize]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/blogs")]
public class BlogsController : ControllerBase
{
    private readonly ISender _sender;

    public BlogsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlog(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBlogQuery(id);

        Result<BlogResponse> result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlog(CreateBlogRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateBlogCommand(
            request.Title,
            request.Slug,
            request.UserId);

        Result<Guid> result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetBlog), new { id = result.Value }, result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBlogs([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {

        var query = new GetAllBlogsQuery(pageIndex, pageSize);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlog(Guid id, UpdateBlogRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateBlogCommand(
            id,
            request.Title,
            request.Slug);

        Result<bool> result = await _sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> SoftDeleteBlog(Guid id)
    {
        var command = new DeleteBlogCommand(id);
        var result = await _sender.Send(command);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

}