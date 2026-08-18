namespace GameDataBackend.Core.Entities;

public class Achievement
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string ConditionType { get; set; } = string.Empty;

    public int ConditionValue { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Game Game { get; set; } = null!;
    public ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
}
