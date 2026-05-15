using System.Collections.Generic;
using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Calendar.Queries.GetActivitiesByMonth;

public class GetActivitiesByMonthResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<CalendarDaySummaryDto> Data { get; set; } = new();
}
