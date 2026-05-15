namespace CleanArchitectureApp.Domain.Entities;

public class HabitTracking
{
    public int Id { get; set; }
    public int HabitId { get; set; }
    public virtual Habit Habit { get; set; }
    public int UserId { get; set; }
    public DateTime RecordDate { get; set; } 
    public decimal ProgressValue { get; set; } 
    public bool IsCompletedForDay { get; set; } 
    public DateTime InsertedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
