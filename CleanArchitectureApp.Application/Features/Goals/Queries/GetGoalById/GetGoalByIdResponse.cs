using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Goals.Queries.GetGoalById;

public class GetGoalByIdResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public GoalDetailDto? Data { get; set; }
}
