using System.Collections.Generic;
using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Habits.Queries.GetActiveHabits;

public class GetActiveHabitsResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<HabitListDto> Data { get; set; } = new();
}
