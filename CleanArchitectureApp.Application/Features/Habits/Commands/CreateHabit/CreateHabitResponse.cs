namespace CleanArchitectureApp.Application.Features.Habits.Commands.CreateHabit;

public class CreateHabitResponse
{
    public int Id { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
