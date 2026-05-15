using System.Security.Claims;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Features.Notifications.Commands.MarkNotificationAsRead;
using CleanArchitectureApp.Application.Features.Notifications.Queries.GetUnreadNotifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureApp.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("unread")]
    public async Task<IActionResult> GetUnreadNotifications()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out int userId))
        {
            return Unauthorized(new { Success = false, Message = "Kullanıcı kimliği alınamadı." });
        }

        var query = new GetUnreadNotificationsQuery { UserId = userId };
        var response = await _mediator.Send(query);

        return Ok(response);
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out int userId))
        {
            return Unauthorized(new { Success = false, Message = "Kullanıcı kimliği alınamadı." });
        }

        var command = new MarkNotificationAsReadCommand { Id = id, UserId = userId };
        var response = await _mediator.Send(command);

        return Ok(response);
    }
}
