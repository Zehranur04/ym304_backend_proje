using CleanArchitectureApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Interfaces.Persistence;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<UserProfile> UserProfiles { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Habit> Habits { get; set; }
    DbSet<HabitTracking> HabitTrackings { get; set; }
    DbSet<Goal> Goals { get; set; }
    DbSet<GoalTracking> GoalTrackings { get; set; }
    DbSet<Notification> Notifications { get; set; }
    DbSet<UserBadge> UserBadges { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade Database { get; }
}
