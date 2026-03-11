using Microsoft.AspNetCore.Identity;
using TechVeo.Authentication.Domain.Entities;
using TechVeo.Authentication.Domain.ValueObjects;
using TechVeo.Authentication.Domain.Repositories;
using TechVeo.Authentication.Integration.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace TechVeo.Authentication.Integration.Tests.Repositories;

public class UserRepositoryTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IntegrationTestFixture _fixture;
    private readonly IUserRepository _userRepository;

    public UserRepositoryTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
        _userRepository = _fixture.ServiceProvider.GetRequiredService<IUserRepository>();
    }

    [Fact(DisplayName = "AddAsync should persist user and return id")]
    [Trait("Integration", "Repositories")]
    public async Task AddAsync_ShouldPersistUserAndReturnId()
    {
        // Arrange
        var passwordHasher = new PasswordHasher<User>();
        var user = new User(new Name("Repo Test User"), "repopuser", "User", new Email("repop@example.com"));
        user.SetPassword(passwordHasher.HashPassword(user, "RepoPassword123!"));

        // Act
        var id = await _userRepository.AddAsync(user);

        // Assert
        id.Should().NotBe(Guid.Empty);

        var persisted = await _userRepository.GetByIdAsync(id);
        persisted.Should().NotBeNull();
        persisted!.Username.Should().Be("repopuser");
        persisted.Name.FullName.Should().Be("Repo Test User");
        persisted.Email.Should().NotBeNull();
        persisted.Email!.Address.Should().Be("repop@example.com");
    }
}
