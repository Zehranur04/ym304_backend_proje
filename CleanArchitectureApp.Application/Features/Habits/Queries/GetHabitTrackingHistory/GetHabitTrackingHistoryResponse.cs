using System.Collections.Generic;
using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Habits.Queries.GetHabitTrackingHistory;

public class GetHabitTrackingHistoryResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<HabitTrackingDto> Data { get; set; } = new List<HabitTrackingDto>();
}
