using System;
using System.Text.Json.Serialization;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Queries.GetHabitTrackingHistory;

public class GetHabitTrackingHistoryQuery : IRequest<GetHabitTrackingHistoryResponse>
{
    public int HabitId { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
