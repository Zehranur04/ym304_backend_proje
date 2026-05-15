namespace CleanArchitectureApp.Application.Features.Habits.Commands.UpdateHabit;

public class UpdateHabitResponse
{
    public int Id { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
