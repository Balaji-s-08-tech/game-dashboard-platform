using GameDataBackend.Core.Entities;
using GameDataBackend.Core.Interfaces.Repositories;
using GameDataBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameDataBackend.Infrastructure.Repositories;

public class AchievementRepository : IAchievementRepository
{
    private readonly AppDbContext _context;

    public AchievementRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Achievement?> GetByIdAsync(Guid id)
    {
        return await _context.Achievements
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Achievement>> GetByGameIdAsync(Guid gameId)
    {
        return await _context.Achievements
            .AsNoTracking()
            .Where(a => a.GameId == gameId)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<Achievement> CreateAsync(Achievement achievement)
    {
        _context.Achievements.Add(achievement);
        await _context.SaveChangesAsync();
        return achievement;
    }
}
