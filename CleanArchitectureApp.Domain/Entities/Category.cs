namespace CleanArchitectureApp.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } 
    public string? IconUrl { get; set; }
    public virtual ICollection<Habit> Habits { get; set; }
    public virtual ICollection<Goal> Goals { get; set; }
}
