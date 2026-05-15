using System;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Calendar.Queries.GetActivitiesByDate;

public class GetActivitiesByDateQuery : IRequest<GetActivitiesByDateResponse>
{
    public int UserId { get; set; }
    public DateTime Date { get; set; }
}
