using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using FluentAssertions;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Domain.Blogs.Common;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Domain.Users;
using Myrtus.CMS.Application.Blogs.Commands.UpdateBlog;
using Myrtus.Clarity.Core.Application.Abstractions.Caching;
using System.Linq.Expressions;
using Myrtus.CMS.Application.Blogs.Commands.CreateBlog;

namespace Myrtus.CMS.Application.Tests.Units.Blogs;

public class UpdateBlogCommandHandlerTests
{
    private readonly Mock<IBlogRepository> _blogRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateBlogCommandHandler _handler;

    public UpdateBlogCommandHandlerTests()
    {
        _blogRepositoryMock = new Mock<IBlogRepository>();
        _cacheServiceMock = new Mock<ICacheService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new UpdateBlogCommandHandler(
            _blogRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _cacheServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenBlogDoesNotExist()
    {
        // Arrange
        var command = new UpdateBlogCommand(Guid.NewGuid(), Guid.NewGuid(), "New Title", "new-slug", "New description");
        _blogRepositoryMock.Setup(repo => repo.GetBlogByIdAsync(
            It.IsAny<Guid>(),
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>(),
            It.IsAny<Expression<Func<Blog, object>>[]>()))
            .ReturnsAsync((Blog)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenBlogIsUpdated()
    {
        // Arrange
        var blogId = Guid.NewGuid();
        var firstName = "Test";
        var lastName = "User";
        var email = "test@example.com";
        var owner = User.Create(firstName, lastName, email);
        var blog = Blog.Create(new Title("Old Title"), new Slug("old-slug"), owner);

        _blogRepositoryMock.Setup(repo => repo.GetBlogByIdAsync(
            blogId,
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>(),
            It.IsAny<Expression<Func<Blog, object>>[]>()))
            .ReturnsAsync(blog);

        var command = new UpdateBlogCommand(blogId, owner.Id, "New Title", "new-slug", "Updated description");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        blog.Title.Value.Should().Be("New Title");
        blog.Slug.Value.Should().Be("new-slug");
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSlugIsAlreadyTaken()
    {
        // Arrange
        var blogId = Guid.NewGuid();
        var owner = User.Create("Test", "User", "test@example.com");
        var blog = Blog.Create(new Title("Old Title"), new Slug("old-slug"), owner);

        _blogRepositoryMock.Setup(repo => repo.GetBlogByIdAsync(
            blogId,
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>(),
            It.IsAny<Expression<Func<Blog, object>>[]>()))
            .ReturnsAsync(blog);

        _blogRepositoryMock.Setup(repo => repo.BlogExistsBySlugAsync(new Slug("new-slug"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new UpdateBlogCommand(blogId, owner.Id, "New Title", "new-slug", "Updated description");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenTitleIsAlreadyTaken()
    {
        // Arrange
        var blogId = Guid.NewGuid();
        var owner = User.Create("Test", "User","test@example.com");
        var blog = Blog.Create(new Title("Old Title"), new Slug("old-slug"), owner);

        _blogRepositoryMock.Setup(repo => repo.GetBlogByIdAsync(
            blogId,
            It.IsAny<bool>(),
            It.IsAny<CancellationToken>(),
            It.IsAny<Expression<Func<Blog, object>>[]>()))
            .ReturnsAsync(blog);

        _blogRepositoryMock.Setup(repo => repo.BlogExistsByTitleAsync(new Title("new-title"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var command = new UpdateBlogCommand(blogId, owner.Id, "new-title", "New Slug", "Updated description");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void UpdateBlogCommandValidator_ShouldHaveValidationError_WhenTitleIsNull()
    {
        // Arrange
        var command = new UpdateBlogCommand(Guid.NewGuid(), Guid.NewGuid(), null, "test-slug", "Updated description");
        var validator = new UpdateBlogCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title" && e.ErrorMessage == "'Title' must not be empty.");
    }

    [Fact]
    public void UpdateBlogCommandValidator_ShouldHaveValidationError_WhenUpdatedByIdIsNull()
    {
        // Arrange
        var command = new UpdateBlogCommand(Guid.NewGuid(), Guid.Empty, "Test Title", "test-slug", "Updated description");
        var validator = new UpdateBlogCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "UpdatedById" && e.ErrorMessage == "'Updated By Id' must not be empty.");
    }

    [Fact]
    public void UpdateBlogCommandValidator_ShouldHaveValidationError_WhenSlugContainsInvalidCharacters()
    {
        // Arrange
        var command = new UpdateBlogCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Title", "invalid-slug!@#", "Updated description");
        var validator = new UpdateBlogCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Slug" && e.ErrorMessage.Contains("Slug contains invalid characters."));
    }

    [Fact]
    public void UpdateBlogCommandValidator_ShouldHaveValidationError_WhenTitleIsWhitespace()
    {
        // Arrange
        var command = new UpdateBlogCommand(Guid.NewGuid(), Guid.NewGuid(), "   ", "test-slug", "Updated description");
        var validator = new UpdateBlogCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title" && e.ErrorMessage == "'Title' must not be empty.");
    }

    [Fact]
    public void UpdateBlogCommandValidator_ShouldHaveValidationError_WhenSlugIsWhitespace()
    {
        // Arrange
        var command = new UpdateBlogCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Title", "   ", "Updated description");
        var validator = new UpdateBlogCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Slug" && e.ErrorMessage == "'Slug' must not be empty.");
    }
}
