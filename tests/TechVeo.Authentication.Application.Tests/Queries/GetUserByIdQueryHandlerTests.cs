using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TechVeo.Authentication.Application.Queries.GetUser;
using TechVeo.Authentication.Domain.Entities;
using TechVeo.Authentication.Domain.Repositories;
using TechVeo.Authentication.Domain.ValueObjects;

namespace TechVeo.Authentication.Application.Tests.Queries;

public class GetUserByIdQueryHandlerTests
{
    [Fact(DisplayName = "Handle should return UserDto when user exists")]
    public async Task Handle_WhenUserExists_ReturnsUserDto()
    {
        // Arrange
        var user = new User(new Name("John Doe"), "johndoe", "User", new Email("john@example.com"));
        var userId = user.Id;

        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);

        var handler = new GetUserByIdQueryHandler(repoMock.Object);
        var query = new GetUserByIdQuery(userId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(userId);
        result.Name.Should().Be(user.Name.FullName);
        result.Username.Should().Be(user.Username);
    }

    [Fact(DisplayName = "Handle should return null when user does not exist")]
    public async Task Handle_WhenUserDoesNotExist_ReturnsNull()
    {
        // Arrange
        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var handler = new GetUserByIdQueryHandler(repoMock.Object);

        // Act
        var result = await handler.Handle(new GetUserByIdQuery(Guid.NewGuid()), CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
