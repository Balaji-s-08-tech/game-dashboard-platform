using System.ComponentModel.DataAnnotations;

namespace GameDataBackend.Core.DTOs.Achievement;

public class CheckAchievementsRequestDto
{
    [Required(ErrorMessage = "GameId is required")]
    public Guid GameId { get; set; }
}

public class AchievementDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ConditionType { get; set; } = string.Empty;
    public int ConditionValue { get; set; }
}

public class UserAchievementDto
{
    public Guid AchievementId { get; set; }
    public string AchievementName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string GameName { get; set; } = string.Empty;
    public DateTime UnlockedAt { get; set; }
}

public class AchievementUnlockResultDto
{
    public int NewlyUnlocked { get; set; }
    public List<UserAchievementDto> UnlockedAchievements { get; set; } = new();
}
