using CleanArchitectureApp.Domain.Enums;

namespace CleanArchitectureApp.Domain.Entities;

public class Goal
{
    public int Id { get; set; }
    public int UserId { get; set; } 
    public virtual User User { get; set; }
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TrackingType TrackingType { get; set; }
    public FrequencyType Frequency { get; set; } 
    public decimal? TargetValue { get; set; } 
    public decimal? CurrentValue { get; set; } 
    public UnitType? Unit { get; set; } 
    public DateTime TargetDate { get; set; } 
    public bool IsArchived { get; set; } = false;
    public bool IsCompleted { get; set; } = false;
    public DateTime? CompletedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime InsertedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual ICollection<GoalTracking> TrackingRecords { get; set; }
}
