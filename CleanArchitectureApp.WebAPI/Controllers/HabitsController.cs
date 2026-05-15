using System.Security.Claims;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Features.Habits.Commands.CreateHabit;
using CleanArchitectureApp.Application.Features.Habits.Commands.DeleteHabit;
using CleanArchitectureApp.Application.Features.Habits.Commands.UpdateHabit;
using CleanArchitectureApp.Application.Features.Habits.Queries.GetActiveHabits;
using CleanArchitectureApp.Application.Features.Habits.Queries.GetHabitById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureApp.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HabitsController : ControllerBase
{
    private readonly IMediator _mediator;

    public HabitsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateHabit([FromBody] CreateHabitCommand command)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        command.UserId = userId;

        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateHabit([FromBody] UpdateHabitCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHabit(int id)
    {
        var command = new DeleteHabitCommand { Id = id };
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteHabit(int id)
    {
        var command = new CleanArchitectureApp.Application.Features.Habits.Commands.CompleteHabit.CompleteHabitCommand { Id = id };
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("completed")]
    public async Task<IActionResult> GetCompletedHabits()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var query = new CleanArchitectureApp.Application.Features.Habits.Queries.GetCompletedHabits.GetCompletedHabitsQuery { UserId = userId };

        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveHabits()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var query = new GetActiveHabitsQuery { UserId = userId };

        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHabitById(int id)
    {
        var query = new GetHabitByIdQuery { Id = id };
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpGet("archived")]
    public async Task<IActionResult> GetArchivedHabits()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var query = new CleanArchitectureApp.Application.Features.Habits.Queries.GetArchivedHabits.GetArchivedHabitsQuery { UserId = userId };
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpPut("{id}/archive")]
    public async Task<IActionResult> ArchiveHabit(int id)
    {
        var command = new CleanArchitectureApp.Application.Features.Habits.Commands.ArchiveHabit.ArchiveHabitCommand { Id = id };
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPut("{id}/unarchive")]
    public async Task<IActionResult> UnarchiveHabit(int id)
    {
        var command = new CleanArchitectureApp.Application.Features.Habits.Commands.UnarchiveHabit.UnarchiveHabitCommand { Id = id };
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("check-expired")]
    public async Task<IActionResult> CheckExpiredWeeklyHabits()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var command = new CleanArchitectureApp.Application.Features.Habits.Commands.CheckExpiredWeeklyHabits.CheckExpiredWeeklyHabitsCommand { UserId = userId };
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
