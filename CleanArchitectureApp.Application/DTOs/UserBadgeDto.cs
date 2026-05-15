using System;

namespace CleanArchitectureApp.Application.DTOs;

public class UserBadgeDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string BadgeName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconData { get; set; } = string.Empty;
    public DateTime EarnedAt { get; set; }
}
