using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Reports.Queries.GetHabitStatistics;

public class GetHabitStatisticsResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public HabitStatisticsDto Data { get; set; } = new();
}
