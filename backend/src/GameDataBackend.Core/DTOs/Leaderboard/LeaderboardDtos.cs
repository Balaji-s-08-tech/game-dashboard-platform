namespace GameDataBackend.Core.DTOs.Leaderboard;

public class LeaderboardEntryDto
{
    public int Rank { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public long Score { get; set; }
    public int Level { get; set; }
}

public class UserRankDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public long Score { get; set; }
    public int Level { get; set; }
    public int Rank { get; set; }
    public int TotalPlayers { get; set; }
}
