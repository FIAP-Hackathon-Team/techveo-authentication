using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechVeo.Authentication.Application.Commands.Register;
using TechVeo.Authentication.Contracts.Authentication;

namespace TechVeo.Authentication.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
[AllowAnonymous]
public class RegisterController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        var command = new RegisterCommand(
            request.FullName,
            request.Username,
            request.Email,
            request.Password);

        var result = await _mediator.Send(command);

        return CreatedAtAction(null, result);
    }
}
