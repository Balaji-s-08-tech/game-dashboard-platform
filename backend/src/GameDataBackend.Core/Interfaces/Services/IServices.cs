using GameDataBackend.Core.DTOs.Auth;
using GameDataBackend.Core.DTOs.Game;
using GameDataBackend.Core.DTOs.Leaderboard;
using GameDataBackend.Core.DTOs.Achievement;
using GameDataBackend.Core.DTOs.Session;

namespace GameDataBackend.Core.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<UserProfileDto> GetProfileAsync(Guid userId);
}

public interface IGameService
{
    Task<GameProgressResponseDto> SaveProgressAsync(Guid userId, SaveGameRequestDto request);
    Task<GameProgressResponseDto?> LoadProgressAsync(Guid userId, Guid gameId);
    Task<List<GameProgressResponseDto>> LoadAllProgressAsync(Guid userId);
}

public interface ILeaderboardService
{
    Task<List<LeaderboardEntryDto>> GetTopPlayersAsync(Guid gameId, int count = 10);
    Task<UserRankDto> GetUserRankAsync(Guid userId, Guid gameId);
}

public interface IAchievementService
{
    Task<AchievementUnlockResultDto> CheckAndUnlockAsync(Guid userId, Guid gameId);
    Task<List<UserAchievementDto>> GetUserAchievementsAsync(Guid userId);
    Task<List<AchievementDto>> GetGameAchievementsAsync(Guid gameId);
}

public interface ISessionService
{
    Task<SessionResponseDto> StartSessionAsync(Guid userId);
    Task<SessionResponseDto> EndSessionAsync(Guid userId, Guid sessionId);
    Task<SessionHistoryResponseDto> GetSessionHistoryAsync(Guid userId);
}
