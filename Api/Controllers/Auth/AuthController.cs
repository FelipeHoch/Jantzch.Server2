using Jantzch.Server2.Application.Auth.AuthUserFromIdp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jantzch.Server2.Api.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{data}")]
    [ProducesResponseType(typeof(string), StatusCodes.Status302Found)]
    public async Task<IActionResult> AuthUserFromIdp(string data)
    {
        var response = await _mediator.Send(new AuthUserFromIdpQuery(data));

        return Redirect(response);
    }
}
