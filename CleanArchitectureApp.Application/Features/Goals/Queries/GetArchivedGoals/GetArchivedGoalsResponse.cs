using System.Collections.Generic;
using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Goals.Queries.GetArchivedGoals;

public class GetArchivedGoalsResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<GoalListDto> Data { get; set; } = new();
}
