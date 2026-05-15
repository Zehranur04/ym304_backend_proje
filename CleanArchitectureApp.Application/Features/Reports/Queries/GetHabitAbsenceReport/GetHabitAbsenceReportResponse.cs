using System.Collections.Generic;
using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Reports.Queries.GetHabitAbsenceReport;

public class GetHabitAbsenceReportResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<HabitAbsenceDto> Data { get; set; } = new();
}
