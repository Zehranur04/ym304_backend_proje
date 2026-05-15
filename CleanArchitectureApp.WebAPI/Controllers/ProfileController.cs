using System.Security.Claims;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Features.Profile.Commands.UpdateUserProfile;
using CleanArchitectureApp.Application.Features.Profile.Queries.GetUserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureApp.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out int userId))
        {
            return Unauthorized(new { Success = false, Message = "Kullanıcı kimliği alınamadı." });
        }

        var query = new GetUserProfileQuery { UserId = userId };
        var response = await _mediator.Send(query);
        
        return Ok(response);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileCommand command)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out int userId))
        {
            return Unauthorized(new { Success = false, Message = "Kullanıcı kimliği alınamadı." });
        }

        command.UserId = userId;
        var response = await _mediator.Send(command);

        return Ok(response);
    }
}
