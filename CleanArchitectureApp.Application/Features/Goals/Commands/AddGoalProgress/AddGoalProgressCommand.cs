using System.Text.Json.Serialization;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.AddGoalProgress;

public class AddGoalProgressCommand : IRequest<AddGoalProgressResponse>
{
    public int GoalId { get; set; }
    public decimal Value { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
}
