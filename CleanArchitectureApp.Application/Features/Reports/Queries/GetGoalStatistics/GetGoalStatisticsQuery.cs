using MediatR;

namespace CleanArchitectureApp.Application.Features.Reports.Queries.GetGoalStatistics;

public class GetGoalStatisticsQuery : IRequest<GetGoalStatisticsResponse>
{
    public int UserId { get; set; }
}
