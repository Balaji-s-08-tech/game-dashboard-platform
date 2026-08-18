using GameDataBackend.Core.DTOs.Achievement;
using GameDataBackend.Core.Entities;
using GameDataBackend.Core.Exceptions;
using GameDataBackend.Core.Interfaces.Repositories;
using GameDataBackend.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace GameDataBackend.Infrastructure.Services;

public class AchievementService : IAchievementService
{
    private readonly IAchievementRepository _achievementRepository;
    private readonly IUserAchievementRepository _userAchievementRepository;
    private readonly IUserGameDataRepository _gameDataRepository;
    private readonly IGameRepository _gameRepository;
    private readonly ILogger<AchievementService> _logger;

    public AchievementService(
        IAchievementRepository achievementRepository,
        IUserAchievementRepository userAchievementRepository,
        IUserGameDataRepository gameDataRepository,
        IGameRepository gameRepository,
        ILogger<AchievementService> logger)
    {
        _achievementRepository = achievementRepository;
        _userAchievementRepository = userAchievementRepository;
        _gameDataRepository = gameDataRepository;
        _gameRepository = gameRepository;
        _logger = logger;
    }

    public async Task<AchievementUnlockResultDto> CheckAndUnlockAsync(Guid userId, Guid gameId)
    {
        _logger.LogInformation("Checking achievements for user {UserId}, game {GameId}", userId, gameId);

        var game = await _gameRepository.GetByIdAsync(gameId)
            ?? throw new NotFoundException("Game", gameId);

        var gameData = await _gameDataRepository.GetByUserAndGameAsync(userId, gameId);

        if (gameData == null)
        {
            return new AchievementUnlockResultDto { NewlyUnlocked = 0 };
        }

        var achievements = await _achievementRepository.GetByGameIdAsync(gameId);

        if (!achievements.Any())
        {
            return new AchievementUnlockResultDto { NewlyUnlocked = 0 };
        }

        var achievementIds = achievements.Select(a => a.Id).ToList();
        var alreadyUnlocked = await _userAchievementRepository.GetUnlockedAchievementIdsAsync(userId, achievementIds);

        var newlyUnlocked = new List<UserAchievementDto>();

        foreach (var achievement in achievements)
        {

            if (alreadyUnlocked.Contains(achievement.Id))
                continue;

            if (EvaluateCondition(achievement, gameData))
            {
                var userAchievement = new UserAchievement
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    AchievementId = achievement.Id,
                    UnlockedAt = DateTime.UtcNow
                };

                await _userAchievementRepository.CreateAsync(userAchievement);

                newlyUnlocked.Add(new UserAchievementDto
                {
                    AchievementId = achievement.Id,
                    AchievementName = achievement.Name,
                    Description = achievement.Description,
                    GameName = game.Name,
                    UnlockedAt = userAchievement.UnlockedAt
                });

                _logger.LogInformation("Achievement unlocked: {AchievementName} for user {UserId}", achievement.Name, userId);
            }
        }

        return new AchievementUnlockResultDto
        {
            NewlyUnlocked = newlyUnlocked.Count,
            UnlockedAchievements = newlyUnlocked
        };
    }

    public async Task<List<UserAchievementDto>> GetUserAchievementsAsync(Guid userId)
    {
        var userAchievements = await _userAchievementRepository.GetByUserIdAsync(userId);

        return userAchievements.Select(ua => new UserAchievementDto
        {
            AchievementId = ua.AchievementId,
            AchievementName = ua.Achievement.Name,
            Description = ua.Achievement.Description,
            GameName = ua.Achievement.Game.Name,
            UnlockedAt = ua.UnlockedAt
        }).ToList();
    }

    public async Task<List<AchievementDto>> GetGameAchievementsAsync(Guid gameId)
    {
        var game = await _gameRepository.GetByIdAsync(gameId)
            ?? throw new NotFoundException("Game", gameId);

        var achievements = await _achievementRepository.GetByGameIdAsync(gameId);

        return achievements.Select(a => new AchievementDto
        {
            Id = a.Id,
            Name = a.Name,
            Description = a.Description,
            ConditionType = a.ConditionType,
            ConditionValue = a.ConditionValue
        }).ToList();
    }

    private static bool EvaluateCondition(Achievement achievement, UserGameData gameData)
    {
        return achievement.ConditionType.ToLowerInvariant() switch
        {
            "score_gte" => gameData.Score >= achievement.ConditionValue,
            "score_lte" => gameData.Score <= achievement.ConditionValue,
            "level_gte" => gameData.Level >= achievement.ConditionValue,
            "level_lte" => gameData.Level <= achievement.ConditionValue,
            _ => false
        };
    }
}
