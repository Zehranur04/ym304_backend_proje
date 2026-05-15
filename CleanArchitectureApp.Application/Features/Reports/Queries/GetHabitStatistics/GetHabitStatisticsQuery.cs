using MediatR;

namespace CleanArchitectureApp.Application.Features.Reports.Queries.GetHabitStatistics;

public class GetHabitStatisticsQuery : IRequest<GetHabitStatisticsResponse>
{
    public int UserId { get; set; }
}
