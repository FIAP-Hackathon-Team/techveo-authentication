using MediatR;
using TechVeo.Authentication.Api.Controllers;
using TechVeo.Authentication.Application.Dto;
using TechVeo.Authentication.Application.Queries.GetUser;

namespace TechVeo.Authentication.Api.Tests.Controllers;

public class UsersControllerTests
{
    [Fact(DisplayName = "GetAsync should return Ok with UserDto when user found")]
    public async Task GetAsync_WhenUserFound_ReturnsOk()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userDto = new UserDto(userId, "John Doe", "johndoe", "john@example.com", "User");

        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), default)).ReturnsAsync(userDto);

        var controller = new UsersController(mediatorMock.Object);

        // Act
        var result = await controller.GetAsync(userId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var ok = result as OkObjectResult;
        ok!.Value.Should().Be(userDto);
    }

    [Fact(DisplayName = "GetAsync should return NotFound when user not found")]
    public async Task GetAsync_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<GetUserByIdQuery>(), default)).ReturnsAsync((UserDto?)null);

        var controller = new UsersController(mediatorMock.Object);

        // Act
        var result = await controller.GetAsync(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
