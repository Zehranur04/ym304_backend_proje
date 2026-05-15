using System.Text.Json.Serialization;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.RemoveGoalProgress;

public class RemoveGoalProgressCommand : IRequest<RemoveGoalProgressResponse>
{
    public int GoalId { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
}
