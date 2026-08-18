namespace GameDataBackend.Core.DTOs.Session;

public class SessionResponseDto
{
    public Guid Id { get; set; }
    public DateTime LoginTime { get; set; }
    public DateTime? LogoutTime { get; set; }
    public double? PlayDurationMinutes { get; set; }
}

public class SessionHistoryResponseDto
{
    public List<SessionResponseDto> Sessions { get; set; } = new();
    public double TotalPlayTimeMinutes { get; set; }
    public int TotalSessions { get; set; }
}
