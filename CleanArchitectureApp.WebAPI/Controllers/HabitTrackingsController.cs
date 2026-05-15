using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Features.Habits.Commands.AddHabitProgress;
using CleanArchitectureApp.Application.Features.Habits.Commands.RemoveHabitProgress;
using CleanArchitectureApp.Application.Features.Habits.Queries.GetHabitTrackingHistory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureApp.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HabitTrackingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public HabitTrackingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddHabitProgress([FromBody] AddHabitProgressCommand command)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userIdString, out int userId))
        {
            command.UserId = userId;
        }

        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHabitTrackingHistory([FromQuery] int habitId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int userId = int.TryParse(userIdString, out int parsedId) ? parsedId : 0;

        var query = new GetHabitTrackingHistoryQuery
        {
            HabitId = habitId,
            UserId = userId,
            StartDate = startDate,
            EndDate = endDate
        };

        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpDelete("undo/{habitId}")]
    public async Task<IActionResult> UndoHabitProgress(int habitId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int userId = int.TryParse(userIdString, out int parsedId) ? parsedId : 0;

        var command = new RemoveHabitProgressCommand
        {
            HabitId = habitId,
            UserId = userId
        };

        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
