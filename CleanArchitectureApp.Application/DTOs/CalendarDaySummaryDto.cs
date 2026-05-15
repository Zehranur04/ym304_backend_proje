using System;

namespace CleanArchitectureApp.Application.DTOs;

public class CalendarDaySummaryDto
{
    public DateTime Date { get; set; }
    public bool HasActivity { get; set; }
}
