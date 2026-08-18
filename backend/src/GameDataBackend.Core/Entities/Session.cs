namespace GameDataBackend.Core.Entities;

public class Session
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime LoginTime { get; set; } = DateTime.UtcNow;
    public DateTime? LogoutTime { get; set; }
    public double? PlayDurationMinutes { get; set; }

    public User User { get; set; } = null!;
}
