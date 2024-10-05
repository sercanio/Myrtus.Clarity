using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using FluentAssertions;
using Myrtus.Clarity.Core.Domain.Abstractions;
using Myrtus.CMS.Application.Blogs.Commands.CreateBlog;
using Myrtus.CMS.Application.Repositories;
using Myrtus.CMS.Domain.Blogs;
using Myrtus.CMS.Domain.Blogs.Common;
using Myrtus.CMS.Application.Abstractionss.Repositories;
using Myrtus.CMS.Domain.Users;
using Myrtus.CMS.Application.Blogs.Queries.GetBlog;

namespace Myrtus.CMS.Application.Tests.Units.Blogs;

public class CreateBlogCommandHandlerTests
{
    private readonly Mock<IBlogRepository> _blogRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateBlogCommandHandler _handler;

    public CreateBlogCommandHandlerTests()
    {
        // Set up the repository mocks
        _blogRepositoryMock = new Mock<IBlogRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        // Pass all required dependencies into the handler
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
        var firstName = new FirstName("Test");
        var lastName = new LastName("User");
        var email = new Email("test@example.com");
        var owner = User.Create(firstName, lastName, email); // Use the Create method

        var command = new CreateBlogCommand("NewTitle", "newslug", ownerId);
        Blog createdBlog = null; // Variable to capture the blog

        _blogRepositoryMock.Setup(repo => repo.BlogExistsByTitleAsync(It.IsAny<Title>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false); // Ensure the title is not already taken
        _blogRepositoryMock.Setup(repo => repo.BlogExistsBySlugAsync(It.IsAny<Slug>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false); // Ensure the slug is not already taken
        _blogRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Blog>()))
            .Callback<Blog>(blog => createdBlog = blog) // Capture the created blog
            .Returns(Task.CompletedTask);

        // Ensure user retrieval returns a valid user
        _userRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(ownerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(owner);

        // Simulate a successful save operation
        _unitOfWorkMock
            .Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1); // Simulate one change

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(new CreateBlogCommandResponse(
            createdBlog.Id,
            createdBlog.Title.Value,
            createdBlog.Slug.Value,
            createdBlog.Owner.Id,
            createdBlog.CreatedOnUtc)); // Assert the blog was created successfully with a valid ID
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
        result.Errors.Should().Contain(e => e.PropertyName == "UserId" && e.ErrorMessage == "'User Id' must not be empty."); // Match exact message
    }

    [Fact]
    public async Task CreateBlogCommandHandler_ShouldReturnFailure_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid(); // Create a non-existing user ID
        var command = new CreateBlogCommand("Test Title", "test-title", userId);

        var title = new Title(command.Title);
        var slug = new Slug(command.Slug);

        // Mock the user retrieval to return null, simulating that the user does not exist
        _userRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(userId, CancellationToken.None))
            .ReturnsAsync((User)null); // Simulate that the user does not exist

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue(); // The handler should fail when the user does not exist
        result.Error.Should().NotBeNull(); // There should be an error message
        result.Error.Should().Be(UserErrors.NotFound); // Ensure correct error is returned

        // Verify that the blog was not added to the repository
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
        result.Errors.Should().Contain(e => e.PropertyName == "Title" && e.ErrorMessage == "'Title' must not be empty."); // Match exact message
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
        result.Errors.Should().Contain(e => e.PropertyName == "Title" && e.ErrorMessage.Contains("The length of 'Title' must be 45 characters or fewer. You entered 46 characters.")); // Match exact message
    }

    [Fact]
    public async Task CreateBlogCommandHandler_ShouldReturnFailure_WhenBlogWithSameTitleExists()
    {
        // Arrange
        var userId = Guid.NewGuid(); // Create a valid user ID
        var command = new CreateBlogCommand("Test Title", "test-title", userId);

        var title = new Title(command.Title);
        var slug = new Slug(command.Slug);

        // Create a mock user instance using the Create method
        var firstName = new FirstName("Test");
        var lastName = new LastName("User");
        var email = new Email("test@example.com");
        var user = User.Create(firstName, lastName, email); // Use the Create method

        // Mock the user retrieval to return a valid user
        _userRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(userId, CancellationToken.None))
            .ReturnsAsync(user); // Simulates that the user exists

        // Mock the repository to return true for BlogExistsByTitleAsync
        // Simulates that the blog title already exists
        _blogRepositoryMock
            .Setup(repo => repo.BlogExistsByTitleAsync(title, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true); // Simulates that the title already exists

        // Simulates that the slug does not exist
        _blogRepositoryMock
            .Setup(repo => repo.BlogExistsBySlugAsync(slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false); // For this scenario, slug is irrelevant since the title check already failed

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue(); // The handler should fail when a blog with the same title exists
        result.Error.Should().NotBeNull(); // There should be an error message
        result.Error.Should().Be(BlogErrors.TitleAlreadyExists); // Ensure correct error is returned

        // Verify that the blog was not added to the repository
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
        result.Errors.Should().Contain(e => e.PropertyName == "Slug" && e.ErrorMessage == "'Slug' must not be empty."); // Match exact message
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
        result.Errors.Should().Contain(e => e.PropertyName == "Slug" && e.ErrorMessage.Contains("The length of 'Slug' must be 45 characters or fewer. You entered 46 characters.")); // Match exact message
    }

    [Fact]
    public async Task CreateBlogCommandHandler_ShouldReturnFailure_WhenBlogWithSameSlugExists()
    {
        // Arrange
        var userId = Guid.NewGuid(); // Create a valid user ID
        var command = new CreateBlogCommand("Test Title", "test-title", userId);

        var title = new Title(command.Title);
        var slug = new Slug(command.Slug);

        // Create a mock user instance using the Create method
        var firstName = new FirstName("Test");
        var lastName = new LastName("User");
        var email = new Email("test@example.com");
        var user = User.Create(firstName, lastName, email); // Use the Create method

        // Mock the user retrieval to return a valid user
        _userRepositoryMock
            .Setup(repo => repo.GetUserByIdAsync(userId, CancellationToken.None))
            .ReturnsAsync(user); // Simulates that the user exists

        // Mock the repository to return false for BlogExistsByTitleAsync
        // Simulates that the title is unique
        _blogRepositoryMock
            .Setup(repo => repo.BlogExistsByTitleAsync(title, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false); // Simulates that the title does not exist

        // Mock the repository to return true for BlogExistsBySlugAsync
        // Simulates that the blog slug already exists
        _blogRepositoryMock
            .Setup(repo => repo.BlogExistsBySlugAsync(slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true); // Simulates that the slug already exists

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue(); // The handler should fail when a blog with the same slug exists
        result.Error.Should().NotBeNull(); // There should be an error message
        result.Error.Should().Be(BlogErrors.SlugAlreadyExists); // Ensure correct error is returned

        // Verify that the blog was not added to the repository
        _blogRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Blog>()), Times.Never);
    }
}
