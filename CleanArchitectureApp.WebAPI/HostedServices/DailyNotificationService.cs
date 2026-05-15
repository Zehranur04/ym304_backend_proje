using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Features.Notifications.Commands.CreateNotification;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using CleanArchitectureApp.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CleanArchitectureApp.WebAPI.HostedServices;

public class DailyNotificationService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DailyNotificationService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var _context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

            var today = DateTime.UtcNow.Date;
            var yesterday = today.AddDays(-1);

            // Senaryo 1: Yaklaşan Hedefler - Zaman Daralıyor (günde bir kez)
            var upcomingGoals = await _context.Goals
                .Where(g => !g.IsCompleted && !g.IsDeleted && g.TargetDate.Date >= today && g.TargetDate.Date <= today.AddDays(3))
                .ToListAsync(stoppingToken);

            foreach (var goal in upcomingGoals)
            {
                bool alreadySent = await _context.Notifications.AnyAsync(
                    n => n.UserId == goal.UserId
                      && n.Title == "⏰ Zaman Daralıyor!"
                      && n.Message.Contains(goal.Title)
                      && n.CreatedAt.Date == today, stoppingToken);

                if (!alreadySent)
                    await _mediator.Send(new CreateNotificationCommand
                    {
                        UserId = goal.UserId,
                        Title = "⏰ Zaman Daralıyor!",
                        Message = $"'{goal.Title}' hedefini tamamlamak için son günlerin! Sona çok yaklaştın, hadi gayret!"
                    }, stoppingToken);
            }

            // Senaryo 2a: Günlük alışkanlıklar — her gün bir kez hatırlatıcı
            var dailyHabits = await _context.Habits
                .Include(h => h.DailyRecords)
                .Where(h => !h.IsCompleted && !h.IsArchived && !h.IsDeleted && h.Frequency == FrequencyType.Daily)
                .ToListAsync(stoppingToken);

            foreach (var habit in dailyHabits)
            {
                var completedToday = habit.DailyRecords.Any(r => r.RecordDate.Date == today && r.IsCompletedForDay);
                if (completedToday) continue;

                bool alreadySentToday = await _context.Notifications.AnyAsync(
                    n => n.UserId == habit.UserId
                      && n.Title == "☀️ Günlük Hatırlatıcı"
                      && n.Message.Contains(habit.Title)
                      && n.CreatedAt.Date == today, stoppingToken);

                if (!alreadySentToday)
                    await _mediator.Send(new CreateNotificationCommand
                    {
                        UserId = habit.UserId,
                        Title = "☀️ Günlük Hatırlatıcı",
                        Message = $"'{habit.Title}' alışkanlığını bugün tamamlamayı unutma! 💪"
                    }, stoppingToken);
            }

            // Senaryo 2b: Haftalık alışkanlıklar — 7 günde bir kez hatırlatıcı
            var weeklyHabits = await _context.Habits
                .Include(h => h.DailyRecords)
                .Where(h => !h.IsCompleted && !h.IsArchived && !h.IsDeleted && h.Frequency == FrequencyType.Weekly)
                .ToListAsync(stoppingToken);

            foreach (var habit in weeklyHabits)
            {
                var daysSinceStart = (today - habit.StartDate.Date).Days;
                if (daysSinceStart < 0 || daysSinceStart % 7 != 0) continue;

                var completedThisWeek = habit.DailyRecords
                    .Any(r => r.RecordDate.Date >= today.AddDays(-6) && r.RecordDate.Date <= today && r.IsCompletedForDay);
                if (completedThisWeek) continue;

                // Bu hafta zaten gönderildi mi?
                bool alreadySentThisWeek = await _context.Notifications.AnyAsync(
                    n => n.UserId == habit.UserId
                      && n.Title == "📅 Haftalık Hatırlatıcı"
                      && n.Message.Contains(habit.Title)
                      && n.CreatedAt.Date >= today.AddDays(-6), stoppingToken);

                if (!alreadySentThisWeek)
                    await _mediator.Send(new CreateNotificationCommand
                    {
                        UserId = habit.UserId,
                        Title = "📅 Haftalık Hatırlatıcı",
                        Message = $"'{habit.Title}' haftalık alışkanlığını bu hafta tamamlamayı unutma! 🗓️"
                    }, stoppingToken);
            }

            // Senaryo 3: Süresi Dolan Hedefler - günde bir kez
            var expiredGoals = await _context.Goals
                .Where(g => !g.IsCompleted && !g.IsDeleted && g.TargetDate.Date == yesterday)
                .ToListAsync(stoppingToken);

            foreach (var goal in expiredGoals)
            {
                bool alreadySent = await _context.Notifications.AnyAsync(
                    n => n.UserId == goal.UserId
                      && n.Title == "⏳ Süren Doldu!"
                      && n.Message.Contains(goal.Title)
                      && n.CreatedAt.Date == today, stoppingToken);

                if (!alreadySent)
                    await _mediator.Send(new CreateNotificationCommand
                    {
                        UserId = goal.UserId,
                        Title = "⏳ Süren Doldu!",
                        Message = $"Maalesef '{goal.Title}' hedefin için belirlediğin süre doldu. Pes etmek yok, hedefini güncelleyip tekrar denemeye ne dersin?"
                    }, stoppingToken);
            }

            // Gece yarısına kadar bekle — böylece her gün tam olarak bir kez çalışır
            var tomorrow = today.AddDays(1);
            var delay = tomorrow - DateTime.UtcNow;
            await Task.Delay(delay > TimeSpan.Zero ? delay : TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
