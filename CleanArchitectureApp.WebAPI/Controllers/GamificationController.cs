using System.Security.Claims;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Features.Gamification.Queries.GetUserBadges;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureApp.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GamificationController : ControllerBase
{
    private readonly IMediator _mediator;

    public GamificationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("my-badges")]
    public async Task<IActionResult> GetMyBadges()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out int userId))
        {
            return Unauthorized(new { Success = false, Message = "Kullanıcı kimliği alınamadı." });
        }

        var query = new GetUserBadgesQuery { UserId = userId };
        var response = await _mediator.Send(query);

        return Ok(response);
    }
}
