using System;

namespace CleanArchitectureApp.Application.DTOs;

public class GoalTrackingDto
{
    public int Id { get; set; }
    public int GoalId { get; set; }
    public DateTime RecordDate { get; set; }
    public decimal ProgressValue { get; set; }
}
