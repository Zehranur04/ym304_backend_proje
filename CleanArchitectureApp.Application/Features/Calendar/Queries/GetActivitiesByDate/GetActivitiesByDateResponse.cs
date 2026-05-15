using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Calendar.Queries.GetActivitiesByDate;

public class GetActivitiesByDateResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DailyActivityDetailDto Data { get; set; } = new();
}
