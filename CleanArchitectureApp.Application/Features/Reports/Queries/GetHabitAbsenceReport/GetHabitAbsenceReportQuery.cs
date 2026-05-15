using MediatR;

namespace CleanArchitectureApp.Application.Features.Reports.Queries.GetHabitAbsenceReport;

public class GetHabitAbsenceReportQuery : IRequest<GetHabitAbsenceReportResponse>
{
    public int UserId { get; set; }
}
