using GameDataBackend.Core.Entities;
using GameDataBackend.Core.Interfaces.Repositories;
using GameDataBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameDataBackend.Infrastructure.Repositories;

public class UserAchievementRepository : IUserAchievementRepository
{
    private readonly AppDbContext _context;

    public UserAchievementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserAchievement>> GetByUserIdAsync(Guid userId)
    {
        return await _context.UserAchievements
            .AsNoTracking()
            .Include(ua => ua.Achievement)
                .ThenInclude(a => a.Game)
            .Where(ua => ua.UserId == userId)
            .OrderByDescending(ua => ua.UnlockedAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(Guid userId, Guid achievementId)
    {
        return await _context.UserAchievements
            .AnyAsync(ua => ua.UserId == userId && ua.AchievementId == achievementId);
    }

    public async Task<UserAchievement> CreateAsync(UserAchievement userAchievement)
    {
        _context.UserAchievements.Add(userAchievement);
        await _context.SaveChangesAsync();
        return userAchievement;
    }

    public async Task<List<Guid>> GetUnlockedAchievementIdsAsync(Guid userId, List<Guid> achievementIds)
    {
        return await _context.UserAchievements
            .Where(ua => ua.UserId == userId && achievementIds.Contains(ua.AchievementId))
            .Select(ua => ua.AchievementId)
            .ToListAsync();
    }
}
