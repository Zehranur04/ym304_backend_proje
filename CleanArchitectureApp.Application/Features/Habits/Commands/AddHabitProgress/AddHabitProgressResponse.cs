namespace CleanArchitectureApp.Application.Features.Habits.Commands.AddHabitProgress;

public class AddHabitProgressResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int Data { get; set; }
    public decimal CurrentAmount { get; set; }
    public int Percentage { get; set; }

    // UC-17: Yeni rozet kazanıldığında frontend'e bildirilir
    public bool HasNewBadge { get; set; }
    public string? NewBadgeName { get; set; }
    public string? NewBadgeDescription { get; set; }
    public string? NewBadgeIcon { get; set; }
}
