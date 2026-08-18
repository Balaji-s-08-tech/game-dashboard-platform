using GameDataBackend.Core.DTOs.Game;
using GameDataBackend.Core.Entities;
using GameDataBackend.Core.Exceptions;
using GameDataBackend.Core.Interfaces.Repositories;
using GameDataBackend.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace GameDataBackend.Infrastructure.Services;

public class GameService : IGameService
{
    private readonly IUserGameDataRepository _gameDataRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IAchievementService _achievementService;
    private readonly ILogger<GameService> _logger;

    public GameService(
        IUserGameDataRepository gameDataRepository,
        IGameRepository gameRepository,
        IAchievementService achievementService,
        ILogger<GameService> logger)
    {
        _gameDataRepository = gameDataRepository;
        _gameRepository = gameRepository;
        _achievementService = achievementService;
        _logger = logger;
    }

    public async Task<GameProgressResponseDto> SaveProgressAsync(Guid userId, SaveGameRequestDto request)
    {
        _logger.LogInformation("Saving progress for user {UserId}, game {GameId}", userId, request.GameId);

        var game = await _gameRepository.GetByIdAsync(request.GameId)
            ?? throw new NotFoundException("Game", request.GameId);

        var existingData = await _gameDataRepository.GetByUserAndGameAsync(userId, request.GameId);

        UserGameData gameData;

        if (existingData != null)
        {

            existingData.Level = request.Level;
            existingData.Score = request.Score;
            existingData.ItemsJson = request.ItemsJson ?? existingData.ItemsJson;
            existingData.IsAutoSave = request.IsAutoSave;
            existingData.SavedAt = DateTime.UtcNow;
            existingData.UpdatedAt = DateTime.UtcNow;

            await _gameDataRepository.UpdateAsync(existingData);
            gameData = existingData;

            _logger.LogInformation("Updated existing progress for user {UserId}, game {GameId}", userId, request.GameId);
        }
        else
        {

            gameData = new UserGameData
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                GameId = request.GameId,
                Level = request.Level,
                Score = request.Score,
                ItemsJson = request.ItemsJson ?? "[]",
                IsAutoSave = request.IsAutoSave,
                SavedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _gameDataRepository.CreateAsync(gameData);
            _logger.LogInformation("Created new progress for user {UserId}, game {GameId}", userId, request.GameId);
        }

        try
        {
            var unlockResult = await _achievementService.CheckAndUnlockAsync(userId, request.GameId);
            if (unlockResult.NewlyUnlocked > 0)
            {
                _logger.LogInformation("Auto-unlocked {Count} achievements for user {UserId}", unlockResult.NewlyUnlocked, userId);
            }
        }
        catch (Exception ex)
        {

            _logger.LogWarning(ex, "Achievement auto-check failed for user {UserId}, game {GameId}", userId, request.GameId);
        }

        return MapToResponse(gameData, game.Name);
    }

    public async Task<GameProgressResponseDto?> LoadProgressAsync(Guid userId, Guid gameId)
    {
        _logger.LogInformation("Loading progress for user {UserId}, game {GameId}", userId, gameId);

        var gameData = await _gameDataRepository.GetByUserAndGameAsync(userId, gameId);

        if (gameData == null)
            return null;

        return MapToResponse(gameData, gameData.Game.Name);
    }

    public async Task<List<GameProgressResponseDto>> LoadAllProgressAsync(Guid userId)
    {
        _logger.LogInformation("Loading all progress for user {UserId}", userId);

        var allData = await _gameDataRepository.GetAllByUserAsync(userId);

        return allData.Select(gd => MapToResponse(gd, gd.Game.Name)).ToList();
    }

    private static GameProgressResponseDto MapToResponse(UserGameData data, string gameName)
    {
        return new GameProgressResponseDto
        {
            Id = data.Id,
            GameId = data.GameId,
            GameName = gameName,
            Level = data.Level,
            Score = data.Score,
            ItemsJson = data.ItemsJson,
            IsAutoSave = data.IsAutoSave,
            SavedAt = data.SavedAt,
            UpdatedAt = data.UpdatedAt
        };
    }
}
