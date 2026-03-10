using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechVeo.Authentication.Application.Queries.GetUser;

namespace TechVeo.Authentication.Controllers;

[ApiController]
[Route("v1/[controller]")]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var query = new GetUserByIdQuery(id);

        var user = await _mediator.Send(query);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}
