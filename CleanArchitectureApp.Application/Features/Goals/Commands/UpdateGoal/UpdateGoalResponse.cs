namespace CleanArchitectureApp.Application.Features.Goals.Commands.UpdateGoal;

public class UpdateGoalResponse
{
    public int Id { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
