using CleanArchitectureApp.Domain.Enums;

namespace CleanArchitectureApp.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public int Age { get; set; }
    public Gender Gender { get; set; }

    // Navigation property
    public UserProfile? UserProfile { get; set; }

    public virtual ICollection<Habit> Habits { get; set; }
    public virtual ICollection<Goal> Goals { get; set; }
}
