using GameDataBackend.Core.Entities;

namespace GameDataBackend.Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<User> CreateAsync(User user);
    Task UpdateAsync(User user);
}

public interface IGameRepository
{
    Task<Game?> GetByIdAsync(Guid id);
    Task<Game?> GetByNameAsync(string name);
    Task<List<Game>> GetAllAsync();
    Task<Game> CreateAsync(Game game);
}

public interface IUserGameDataRepository
{
    Task<UserGameData?> GetByUserAndGameAsync(Guid userId, Guid gameId);
    Task<List<UserGameData>> GetAllByUserAsync(Guid userId);
    Task<UserGameData> CreateAsync(UserGameData data);
    Task UpdateAsync(UserGameData data);

    Task<List<UserGameData>> GetTopScoresAsync(Guid gameId, int count);
    Task<int> GetUserRankAsync(Guid gameId, long userScore);
    Task<int> GetTotalPlayersAsync(Guid gameId);
}

public interface IAchievementRepository
{
    Task<Achievement?> GetByIdAsync(Guid id);
    Task<List<Achievement>> GetByGameIdAsync(Guid gameId);
    Task<Achievement> CreateAsync(Achievement achievement);
}

public interface IUserAchievementRepository
{
    Task<List<UserAchievement>> GetByUserIdAsync(Guid userId);
    Task<bool> ExistsAsync(Guid userId, Guid achievementId);
    Task<UserAchievement> CreateAsync(UserAchievement userAchievement);
    Task<List<Guid>> GetUnlockedAchievementIdsAsync(Guid userId, List<Guid> achievementIds);
}

public interface ISessionRepository
{
    Task<Session?> GetByIdAsync(Guid id);
    Task<List<Session>> GetByUserIdAsync(Guid userId);
    Task<Session> CreateAsync(Session session);
    Task UpdateAsync(Session session);
    Task<Session?> GetActiveSessionAsync(Guid userId);
}
