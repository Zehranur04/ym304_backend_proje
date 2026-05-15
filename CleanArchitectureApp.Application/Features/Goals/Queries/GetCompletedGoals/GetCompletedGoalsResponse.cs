using System.Collections.Generic;
using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Goals.Queries.GetCompletedGoals;

public class GetCompletedGoalsResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<GoalListDto> Data { get; set; } = new();
}
