using MediatR;

namespace CleanArchitectureApp.Application.Features.Calendar.Queries.GetActivitiesByMonth;

public class GetActivitiesByMonthQuery : IRequest<GetActivitiesByMonthResponse>
{
    public int UserId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
}
