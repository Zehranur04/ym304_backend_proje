using System.Text.Json.Serialization;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.RemoveHabitProgress;

public class RemoveHabitProgressCommand : IRequest<RemoveHabitProgressResponse>
{
    public int HabitId { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
}
