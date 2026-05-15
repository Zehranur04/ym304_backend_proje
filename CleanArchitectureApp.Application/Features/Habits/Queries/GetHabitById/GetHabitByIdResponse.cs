using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Habits.Queries.GetHabitById;

public class GetHabitByIdResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public HabitDetailDto? Data { get; set; }
}
