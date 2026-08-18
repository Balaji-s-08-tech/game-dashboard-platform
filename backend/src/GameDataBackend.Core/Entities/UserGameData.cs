namespace GameDataBackend.Core.Entities;

public class UserGameData
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    public int Level { get; set; }
    public long Score { get; set; }
    public string ItemsJson { get; set; } = "[]";
    public bool IsAutoSave { get; set; }
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Game Game { get; set; } = null!;
}
