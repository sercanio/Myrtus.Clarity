using Moq;
using FluentAssertions;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Blogs.Commands.CreateBlog;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Domain.Blogs.Common;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Domain.Users;

namespace Myrtus.CMS.Application.Tests.Units.Blogs;

public class CreateBlogCommandHandlerTests
{
    private readonly Mock<IBlogRepository> _blogRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateBlogCommandHandler _handler;

    public CreateBlogCommandHandlerTests()
    {
        _blogRepositoryMock = new Mock<IBlogRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _handler = new CreateBlogCommandHandler(
            _blogRepositoryMock.Object,
            _userRepositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task CreateBlogCommandHandler_ShouldReturnSuccess_WhenBlogIsCreated()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var firstName = "Test";
        var lastName = "User";
        var email = "test@example.com";
        var owner = User.Create(firstName, lastName, email);

        var command = new CreateBlogCommand("NewTitle", "newslug", ownerId);
        Blog createdBlog = null;

        _blogRepositoryMock.Setup(repo => repo.BlogExistsByTitleAsync(It.IsAny<Title>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _blogRepositoryMock.Setup(repo => repo.BlogExistsBySlugAsync(It.IsAny<Slug>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _blogRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Blog>()))
            .Callback<Blog>(blog => createdBlog = blog)
            .Returns(Task.CompletedTask);

        _userRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(ownerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(owner);

        _unitOfWorkMock
            .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(new CreateBlogCommandResponse(
            createdBlog.Id,
            createdBlog.Title.Value,
            createdBlog.Slug.Value,
            createdBlog.Owner.Id,
            createdBlog.CreatedOnUtc));
        _blogRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Blog>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void CreateBlogCommandValidator_ShouldHaveValidationError_WhenUserIdIsEmpty()
    {
        // Arrange
        var command = new CreateBlogCommand("Test Title", "test-slug", Guid.Empty);
        var validator = new CreateBlogCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "UserId" && e.ErrorMessage == "'User Id' must not be empty.");
    }

    [Fact]
    public async Task CreateBlogCommandHandler_ShouldReturnFailure_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateBlogCommand("Test Title", "test-title", userId);

        var title = new Title(command.Title);
        var slug = new Slug(command.Slug);

        _userRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(userId, CancellationToken.None))
            .ReturnsAsync((User)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();

        _blogRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Blog>()), Times.Never);
    }

    [Fact]
    public void CreateBlogCommandValidator_ShouldHaveValidationError_WhenTitleIsEmpty()
    {
        // Arrange
        var command = new CreateBlogCommand("", "test-slug", Guid.NewGuid());
        var validator = new CreateBlogCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title" && e.ErrorMessage == "'Title' must not be empty.");
    }

    [Fact]
    public void CreateBlogCommandValidator_ShouldHaveValidationError_WhenTitleExceedsMaxLength()
    {
        // Arrange
        var longTitle = new string('a', 46);
        var command = new CreateBlogCommand(longTitle, "test-slug", Guid.NewGuid());
        var validator = new CreateBlogCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Title" && e.ErrorMessage.Contains("The length of 'Title' must be 45 characters or fewer. You entered 46 characters."));
    }

    [Fact]
    public async Task CreateBlogCommandHandler_ShouldReturnFailure_WhenBlogWithSameTitleExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateBlogCommand("Test Title", "test-title", userId);

        var title = new Title(command.Title);
        var slug = new Slug(command.Slug);

        var firstName = "Test";
        var lastName = "User";
        var email = "test@example.com";
        var user = User.Create(firstName, lastName, email);

        _userRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(userId, CancellationToken.None))
            .ReturnsAsync(user);

        _blogRepositoryMock
            .Setup(repo => repo.BlogExistsByTitleAsync(title, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _blogRepositoryMock
            .Setup(repo => repo.BlogExistsBySlugAsync(slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();

        _blogRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Blog>()), Times.Never);
    }

    [Fact]
    public void CreateBlogCommandValidator_ShouldHaveValidationError_WhenSlugIsEmpty()
    {
        // Arrange
        var command = new CreateBlogCommand("Test Title", "", Guid.NewGuid());
        var validator = new CreateBlogCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Slug" && e.ErrorMessage == "'Slug' must not be empty.");
    }

    [Fact]
    public void CreateBlogCommandValidator_ShouldHaveValidationError_WhenSlugExceedsMaxLength()
    {
        // Arrange
        var longSlug = new string('a', 46);
        var command = new CreateBlogCommand("Test Title", longSlug, Guid.NewGuid());
        var validator = new CreateBlogCommandValidator();

        // Act
        var result = validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Slug" && e.ErrorMessage.Contains("The length of 'Slug' must be 45 characters or fewer. You entered 46 characters."));
    }

    [Fact]
    public async Task CreateBlogCommandHandler_ShouldReturnFailure_WhenBlogWithSameSlugExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateBlogCommand("Test Title", "test-title", userId);

        var title = new Title(command.Title);
        var slug = new Slug(command.Slug);

        var firstName = "Test";
        var lastName = "User";
        var email = "test@example.com";
        var user = User.Create(firstName, lastName, email);

        _userRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(userId, CancellationToken.None))
            .ReturnsAsync(user);

        _blogRepositoryMock
            .Setup(repo => repo.BlogExistsByTitleAsync(title, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _blogRepositoryMock
            .Setup(repo => repo.BlogExistsBySlugAsync(slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNull();

        _blogRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Blog>()), Times.Never);
    }
}