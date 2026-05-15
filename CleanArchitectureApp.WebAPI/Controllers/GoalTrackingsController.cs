using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Features.Goals.Commands.AddGoalProgress;
using CleanArchitectureApp.Application.Features.Goals.Commands.RemoveGoalProgress;
using CleanArchitectureApp.Application.Features.Goals.Queries.GetGoalTrackingHistory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureApp.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GoalTrackingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GoalTrackingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddGoalProgress([FromBody] AddGoalProgressCommand command)
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
    public async Task<IActionResult> GetGoalTrackingHistory([FromQuery] int goalId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int userId = int.TryParse(userIdString, out int parsedId) ? parsedId : 0;

        var query = new GetGoalTrackingHistoryQuery
        {
            GoalId = goalId,
            UserId = userId,
            StartDate = startDate,
            EndDate = endDate
        };

        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpDelete("undo/{goalId}")]
    public async Task<IActionResult> UndoGoalProgress(int goalId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int userId = int.TryParse(userIdString, out int parsedId) ? parsedId : 0;

        var command = new RemoveGoalProgressCommand
        {
            GoalId = goalId,
            UserId = userId
        };

        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
