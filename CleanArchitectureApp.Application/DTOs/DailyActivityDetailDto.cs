using System.Collections.Generic;

namespace CleanArchitectureApp.Application.DTOs;

public class DailyActivityDetailDto
{
    public List<CalendarEventDto> Activities { get; set; } = new();
}
