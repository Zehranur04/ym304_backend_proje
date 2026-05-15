using System.Text.Json.Serialization;
using MediatR;

namespace CleanArchitectureApp.Application.Features.Habits.Commands.AddHabitProgress;

public class AddHabitProgressCommand : IRequest<AddHabitProgressResponse>
{
    public int HabitId { get; set; }
    public decimal Value { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
}
