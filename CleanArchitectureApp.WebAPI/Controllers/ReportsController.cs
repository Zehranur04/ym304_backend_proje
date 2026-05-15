using System.Security.Claims;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Features.Reports.Queries.GetHabitAbsenceReport;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureApp.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("habit-absences")]
    public async Task<IActionResult> GetHabitAbsenceReport()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out int userId))
        {
            return Unauthorized(new { Success = false, Message = "Kullanıcı kimliği alınamadı." });
        }

        var query = new GetHabitAbsenceReportQuery { UserId = userId };
        var response = await _mediator.Send(query);

        return Ok(response);
    }

    [HttpGet("goal-statistics")]
    public async Task<IActionResult> GetGoalStatistics()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out int userId))
        {
            return Unauthorized(new { Success = false, Message = "Kullanıcı kimliği alınamadı." });
        }

        var query = new CleanArchitectureApp.Application.Features.Reports.Queries.GetGoalStatistics.GetGoalStatisticsQuery { UserId = userId };
        var response = await _mediator.Send(query);

        return Ok(response);
    }

    [HttpGet("habit-statistics")]
    public async Task<IActionResult> GetHabitStatistics()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdString, out int userId))
        {
            return Unauthorized(new { Success = false, Message = "Kullanıcı kimliği alınamadı." });
        }

        var query = new CleanArchitectureApp.Application.Features.Reports.Queries.GetHabitStatistics.GetHabitStatisticsQuery { UserId = userId };
        var response = await _mediator.Send(query);

        return Ok(response);
    }
}
