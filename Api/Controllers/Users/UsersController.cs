using Jantzch.Server2.Application.Users;
using Jantzch.Server2.Application.Users.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jantzch.Server2.Api.Controllers.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers([FromQuery] UsersResourceParameters parameters, CancellationToken cancellationToken)
    {
        var users = await _mediator.Send(new UsersQuery(parameters), cancellationToken);

        return Ok(users);
    }
}
