using System;
using System.Text.Json.Serialization;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Queries.GetGoalTrackingHistory;

public class GetGoalTrackingHistoryQuery : IRequest<GetGoalTrackingHistoryResponse>
{
    public int GoalId { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
