using System;

namespace CleanArchitectureApp.Application.DTOs;

public class HabitTrackingDto
{
    public int Id { get; set; }
    public int HabitId { get; set; }
    public DateTime RecordDate { get; set; }
    public decimal ProgressValue { get; set; }
    public bool IsCompletedForDay { get; set; }
}
