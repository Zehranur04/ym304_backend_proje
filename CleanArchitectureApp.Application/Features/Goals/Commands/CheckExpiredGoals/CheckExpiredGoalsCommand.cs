using System.Text.Json.Serialization;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Goals.Commands.CheckExpiredGoals;

public class CheckExpiredGoalsCommand : IRequest<CheckExpiredGoalsResponse>
{
    [JsonIgnore]
    public int UserId { get; set; }
}
