using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Features.Calendar.Queries.GetActivitiesByDate;
using CleanArchitectureApp.Application.Features.Calendar.Queries.GetActivitiesByMonth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureApp.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CalendarController : ControllerBase
{
    private readonly IMediator _mediator;

    public CalendarController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("month-summary")]
    public async Task<IActionResult> GetActivitiesByMonth([FromQuery] int year, [FromQuery] int month)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out int userId))
        {
            return Unauthorized(new { Success = false, Message = "Kullanıcı kimliği alınamadı." });
        }

        var query = new GetActivitiesByMonthQuery { UserId = userId, Year = year, Month = month };
        var response = await _mediator.Send(query);

        return Ok(response);
    }

    [HttpGet("day-details")]
    public async Task<IActionResult> GetActivitiesByDate([FromQuery] DateTime date)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out int userId))
        {
            return Unauthorized(new { Success = false, Message = "Kullanıcı kimliği alınamadı." });
        }

        var query = new GetActivitiesByDateQuery { UserId = userId, Date = date };
        var response = await _mediator.Send(query);

        return Ok(response);
    }
}
