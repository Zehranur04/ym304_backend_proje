using System.Collections.Generic;
using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Goals.Queries.GetGoalTrackingHistory;

public class GetGoalTrackingHistoryResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<GoalTrackingDto> Data { get; set; } = new List<GoalTrackingDto>();
}
