using CleanArchitectureApp.Application.Interfaces.Persistence;
using CleanArchitectureApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserProfile> UserProfiles { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Habit> Habits { get; set; } = null!;
    public DbSet<HabitTracking> HabitTrackings { get; set; } = null!;
    public DbSet<Goal> Goals { get; set; } = null!;
    public DbSet<GoalTracking> GoalTrackings { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<UserBadge> UserBadges { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure 1:1 relationship
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserProfile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId);

        // Global Query Filters
        modelBuilder.Entity<Habit>().HasQueryFilter(h => !h.IsDeleted);
        modelBuilder.Entity<Goal>().HasQueryFilter(g => !g.IsDeleted);

        // Data Seeding
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Sağlık & Yaşam" },      // Örn: Su içmek, düzenli uyku
            new Category { Id = 2, Name = "Spor & Egzersiz" },     // Örn: Fitness, yürüyüş, yoga
            new Category { Id = 3, Name = "Eğitim & Öğrenim" },    // Örn: Okul dersleri, dil öğrenmek
            new Category { Id = 4, Name = "Kariyer & İş" },        // Örn: Proje bitirmek, terfi almak
            new Category { Id = 5, Name = "Kişisel Gelişim" },     // Örn: Kitap okumak, podcast dinlemek
            new Category { Id = 6, Name = "Eğlence & Sosyal" },    // Örn: Oyun oynamak, arkadaşlarla buluşmak
            new Category { Id = 7, Name = "Finans & Birikim" },    // Örn: 10.000 TL biriktirmek, yatırım yapmak
            new Category { Id = 8, Name = "Aile & İlişkiler" },    // Örn: Aileyle vakit geçirmek, eşe sürpriz
            new Category { Id = 9, Name = "Sanat & Hobiler" },     // Örn: Gitar çalmak, resim yapmak
            new Category { Id = 10, Name = "Ev & Düzen" },         // Örn: Oda toplamak, bitki bakımı
            new Category { Id = 11, Name = "Seyahat & Keşif" },    // Örn: Yeni bir şehir görmek, kampa gitmek
            new Category { Id = 12, Name = "Diğer" }               // Hiçbirine uymayan çılgın hedefler için hayat kurtarıcı :)
        );
    }
}
