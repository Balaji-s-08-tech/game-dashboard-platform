namespace GameDataBackend.Core.Entities;

public class UserAchievement
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid AchievementId { get; set; }
    public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Achievement Achievement { get; set; } = null!;
}
