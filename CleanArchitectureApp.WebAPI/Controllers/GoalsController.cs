using System.Security.Claims;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Features.Goals.Commands.CheckExpiredGoals;
using CleanArchitectureApp.Application.Features.Goals.Commands.CompleteGoal;
using CleanArchitectureApp.Application.Features.Goals.Commands.CreateGoal;
using CleanArchitectureApp.Application.Features.Goals.Commands.DeleteGoal;
using CleanArchitectureApp.Application.Features.Goals.Commands.UpdateGoal;
using CleanArchitectureApp.Application.Features.Goals.Queries.GetActiveGoals;
using CleanArchitectureApp.Application.Features.Goals.Queries.GetGoalById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureApp.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GoalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GoalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateGoal([FromBody] CreateGoalCommand command)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        command.UserId = userId;

        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateGoal([FromBody] UpdateGoalCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGoal(int id)
    {
        var command = new DeleteGoalCommand { Id = id };
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteGoal(int id)
    {
        var command = new CompleteGoalCommand { Id = id };
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("completed")]
    public async Task<IActionResult> GetCompletedGoals()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var query = new CleanArchitectureApp.Application.Features.Goals.Queries.GetCompletedGoals.GetCompletedGoalsQuery { UserId = userId };

        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveGoals()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var query = new GetActiveGoalsQuery { UserId = userId };

        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGoalById(int id)
    {
        var query = new GetGoalByIdQuery { Id = id };
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpPost("check-expired")]
    public async Task<IActionResult> CheckExpiredGoals()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        int userId = int.TryParse(userIdString, out int parsedId) ? parsedId : 0;

        var command = new CheckExpiredGoalsCommand { UserId = userId };
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("archived")]
    public async Task<IActionResult> GetArchivedGoals()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var query = new CleanArchitectureApp.Application.Features.Goals.Queries.GetArchivedGoals.GetArchivedGoalsQuery { UserId = userId };
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpPut("{id}/archive")]
    public async Task<IActionResult> ArchiveGoal(int id)
    {
        var command = new CleanArchitectureApp.Application.Features.Goals.Commands.ArchiveGoal.ArchiveGoalCommand { Id = id };
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPut("{id}/unarchive")]
    public async Task<IActionResult> UnarchiveGoal(int id)
    {
        var command = new CleanArchitectureApp.Application.Features.Goals.Commands.UnarchiveGoal.UnarchiveGoalCommand { Id = id };
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
