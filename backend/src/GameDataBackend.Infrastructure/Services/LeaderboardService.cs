using GameDataBackend.Core.DTOs.Leaderboard;
using GameDataBackend.Core.Exceptions;
using GameDataBackend.Core.Interfaces.Repositories;
using GameDataBackend.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace GameDataBackend.Infrastructure.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly IUserGameDataRepository _gameDataRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<LeaderboardService> _logger;

    public LeaderboardService(
        IUserGameDataRepository gameDataRepository,
        IGameRepository gameRepository,
        IUserRepository userRepository,
        ILogger<LeaderboardService> logger)
    {
        _gameDataRepository = gameDataRepository;
        _gameRepository = gameRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<List<LeaderboardEntryDto>> GetTopPlayersAsync(Guid gameId, int count = 10)
    {
        _logger.LogInformation("Fetching top {Count} players for game {GameId}", count, gameId);

        var game = await _gameRepository.GetByIdAsync(gameId)
            ?? throw new NotFoundException("Game", gameId);

        count = Math.Clamp(count, 1, 100);

        var topScores = await _gameDataRepository.GetTopScoresAsync(gameId, count);

        return topScores.Select((entry, index) => new LeaderboardEntryDto
        {
            Rank = index + 1,
            UserId = entry.UserId,
            Username = entry.User.Username,
            Score = entry.Score,
            Level = entry.Level
        }).ToList();
    }

    public async Task<UserRankDto> GetUserRankAsync(Guid userId, Guid gameId)
    {
        _logger.LogInformation("Fetching rank for user {UserId} in game {GameId}", userId, gameId);

        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new NotFoundException("User", userId);

        var gameData = await _gameDataRepository.GetByUserAndGameAsync(userId, gameId)
            ?? throw new NotFoundException($"No game progress found for user '{userId}' in game '{gameId}'.");

        var rank = await _gameDataRepository.GetUserRankAsync(gameId, gameData.Score);
        var totalPlayers = await _gameDataRepository.GetTotalPlayersAsync(gameId);

        return new UserRankDto
        {
            UserId = userId,
            Username = user.Username,
            Score = gameData.Score,
            Level = gameData.Level,
            Rank = rank,
            TotalPlayers = totalPlayers
        };
    }
}
