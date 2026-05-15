namespace CleanArchitectureApp.Domain.Entities;

public class UserProfile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Bio { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public int TotalScore { get; set; } = 0;

    // Navigation property
    public User User { get; set; } = null!;
}
