namespace CleanArchitectureApp.Application.Features.Goals.Commands.CreateGoal;

public class CreateGoalResponse
{
    public int Id { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
