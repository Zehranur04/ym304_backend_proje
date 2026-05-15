namespace CleanArchitectureApp.Domain.Entities;

public class GoalTracking
{
    public int Id { get; set; }
    public int GoalId { get; set; }
    public virtual Goal Goal { get; set; }
    public int UserId { get; set; }
    public DateTime RecordDate { get; set; } 
    public decimal ProgressValue { get; set; } 
    public bool IsCompletedForInterval { get; set; } 
    public DateTime InsertedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
