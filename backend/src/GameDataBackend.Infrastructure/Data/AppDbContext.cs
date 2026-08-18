using GameDataBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameDataBackend.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<UserGameData> UserGameData => Set<UserGameData>();
    public DbSet<Achievement> Achievements => Set<Achievement>();
    public DbSet<UserAchievement> UserAchievements => Set<UserAchievement>();
    public DbSet<Session> Sessions => Set<Session>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        var game1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var game2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var game3Id = Guid.Parse("33333333-3333-3333-3333-333333333333");

        modelBuilder.Entity<Game>().HasData(
            new Game { Id = game1Id, Name = "Dragon Quest", Description = "Epic RPG adventure game", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Game { Id = game2Id, Name = "Space Blaster", Description = "Arcade space shooter", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Game { Id = game3Id, Name = "Puzzle Master", Description = "Brain-teasing puzzle game", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<Achievement>().HasData(

            new Achievement { Id = Guid.Parse("aaaa0001-0000-0000-0000-000000000001"), GameId = game1Id, Name = "First Steps", Description = "Reach level 5", ConditionType = "level_gte", ConditionValue = 5, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Achievement { Id = Guid.Parse("aaaa0002-0000-0000-0000-000000000002"), GameId = game1Id, Name = "Veteran Warrior", Description = "Reach level 25", ConditionType = "level_gte", ConditionValue = 25, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Achievement { Id = Guid.Parse("aaaa0003-0000-0000-0000-000000000003"), GameId = game1Id, Name = "Score Hunter", Description = "Score 10,000 points", ConditionType = "score_gte", ConditionValue = 10000, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Achievement { Id = Guid.Parse("aaaa0004-0000-0000-0000-000000000004"), GameId = game1Id, Name = "Legend", Description = "Score 100,000 points", ConditionType = "score_gte", ConditionValue = 100000, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            new Achievement { Id = Guid.Parse("bbbb0001-0000-0000-0000-000000000001"), GameId = game2Id, Name = "Cadet", Description = "Reach level 3", ConditionType = "level_gte", ConditionValue = 3, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Achievement { Id = Guid.Parse("bbbb0002-0000-0000-0000-000000000002"), GameId = game2Id, Name = "Ace Pilot", Description = "Score 50,000 points", ConditionType = "score_gte", ConditionValue = 50000, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            new Achievement { Id = Guid.Parse("cccc0001-0000-0000-0000-000000000001"), GameId = game3Id, Name = "Quick Thinker", Description = "Reach level 10", ConditionType = "level_gte", ConditionValue = 10, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Achievement { Id = Guid.Parse("cccc0002-0000-0000-0000-000000000002"), GameId = game3Id, Name = "Puzzle Genius", Description = "Score 25,000 points", ConditionType = "score_gte", ConditionValue = 25000, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
