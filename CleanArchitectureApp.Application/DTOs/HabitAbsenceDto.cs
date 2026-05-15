using System;

namespace CleanArchitectureApp.Application.DTOs;

public class HabitAbsenceDto
{
    public int HabitId { get; set; }
    public string HabitTitle { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
