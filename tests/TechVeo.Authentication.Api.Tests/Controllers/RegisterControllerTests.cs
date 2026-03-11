using MediatR;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TechVeo.Authentication.Api.Controllers;
using TechVeo.Authentication.Application.Commands.Register;
using TechVeo.Authentication.Application.Dto;
using TechVeo.Authentication.Contracts.Authentication;

namespace TechVeo.Authentication.Api.Tests.Controllers;

public class RegisterControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly RegisterController _controller;

    public RegisterControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new RegisterController(_mediatorMock.Object);
    }

    [Fact(DisplayName = "RegisterAsync should return Created with user dto")]
    [Trait("Api", "RegisterController")]
    public async Task RegisterAsync_WithValidRequest_ShouldReturnCreatedWithUserDto()
    {
        // Arrange
        var request = new RegisterRequest("New User", "newuser", "newuser@example.com", "Password123!");
        var expectedResult = new UserDto(Guid.NewGuid(), "New User", "newuser", "newuser@example.com", "User");

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.RegisterAsync(request);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var created = result as CreatedAtActionResult;
        created!.Value.Should().Be(expectedResult);

        _mediatorMock.Verify(x => x.Send(
            It.Is<RegisterCommand>(cmd =>
                cmd.FullName == "New User" &&
                cmd.Username == "newuser" &&
                cmd.Email == "newuser@example.com" &&
                cmd.Password == "Password123!"),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact(DisplayName = "RegisterAsync should forward command to mediator")]
    [Trait("Api", "RegisterController")]
    public async Task RegisterAsync_ShouldForwardCommandToMediator()
    {
        // Arrange
        var fullName = "Another User";
        var username = "anotheruser";
        var email = "another@example.com";
        var password = "S3cureP@ss!";

        var request = new RegisterRequest(fullName, username, email, password);
        var expectedResult = new UserDto(Guid.NewGuid(), fullName, username, email, "User");

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.RegisterAsync(request);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        _mediatorMock.Verify(x => x.Send(
            It.Is<RegisterCommand>(cmd => cmd.FullName == fullName && cmd.Username == username && cmd.Email == email && cmd.Password == password),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
