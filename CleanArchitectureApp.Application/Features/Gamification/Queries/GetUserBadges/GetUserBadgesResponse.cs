using System.Collections.Generic;
using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Gamification.Queries.GetUserBadges;

public class GetUserBadgesResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<UserBadgeDto> Data { get; set; } = new();
}
