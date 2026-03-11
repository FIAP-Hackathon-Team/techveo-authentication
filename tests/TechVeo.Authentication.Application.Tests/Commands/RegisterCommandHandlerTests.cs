using TechVeo.Authentication.Application.Commands.Register;
using TechVeo.Authentication.Domain.Entities;
using TechVeo.Authentication.Domain.Repositories;
using TechVeo.Authentication.Domain.ValueObjects;

namespace TechVeo.Authentication.Application.Tests.Commands;

public class RegisterCommandHandlerTests
{
    [Fact(DisplayName = "Handle should throw when user already exists")]
    public async Task Handle_WhenUserExists_ShouldThrowApplicationException()
    {
        // Arrange
        var repoMock = new Mock<IUserRepository>();
        var existingUser = new User(new Name("Existing User"), "existinguser", "User", new Email("existing@example.com"));
        repoMock.Setup(r => r.GetByUsernameOrEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(existingUser);

        var handler = new RegisterCommandHandler(repoMock.Object);
        var command = new RegisterCommand("Existing User", "existinguser", "existing@example.com", "Password123!");

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Shared.Application.Exceptions.ApplicationException>();
        repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact(DisplayName = "Handle should add new user and return UserDto")]
    public async Task Handle_WhenNewUser_ShouldAddAndReturnUserDto()
    {
        // Arrange
        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetByUsernameOrEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var expectedId = Guid.NewGuid();
        User? captured = null;
        repoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(u => captured = u)
            .ReturnsAsync(expectedId);

        var handler = new RegisterCommandHandler(repoMock.Object);
        var command = new RegisterCommand("New User", "newuser", "newuser@example.com", "Password123!");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expectedId);
        result.Name.Should().Be("New User");
        result.Username.Should().Be("newuser");
        result.Email.Should().Be("newuser@example.com");
        result.Role.Should().Be("user");

        captured.Should().NotBeNull();
        captured!.Username.Should().Be("newuser");
        captured.Name.FullName.Should().Be("New User");
        captured.PasswordHash.Should().NotBeNullOrWhiteSpace();

        repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }
}
