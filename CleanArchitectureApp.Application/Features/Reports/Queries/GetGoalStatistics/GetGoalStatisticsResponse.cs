using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Reports.Queries.GetGoalStatistics;

public class GetGoalStatisticsResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public GoalStatisticsDto Data { get; set; } = new();
}
