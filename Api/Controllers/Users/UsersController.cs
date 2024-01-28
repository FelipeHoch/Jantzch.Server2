using Jantzch.Server2.Application.Users;
using Jantzch.Server2.Application.Users.CreateUser;
using Jantzch.Server2.Application.Users.DeleteUser;
using Jantzch.Server2.Application.Users.EditUser;
using Jantzch.Server2.Application.Users.GetUsers;
using Jantzch.Server2.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
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

    [HttpPost]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUser([FromQuery] string data, [FromBody] User user, CancellationToken cancellationToken)
    {
        var userCreated = await _mediator.Send(new CreateUserCommand(data, user), cancellationToken);

        return Ok(userCreated);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<IActionResult> EditUser(string id, [FromBody] JsonPatchDocument<User> model, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new EditUserCommand.Command(model, id), cancellationToken);

        return Ok(user);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteUser(string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteUserCommand(id), cancellationToken);

        return NoContent();
    }
}
