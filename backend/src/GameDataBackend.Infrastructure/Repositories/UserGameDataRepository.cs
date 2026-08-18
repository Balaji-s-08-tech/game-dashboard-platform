using GameDataBackend.Core.Entities;
using GameDataBackend.Core.Interfaces.Repositories;
using GameDataBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameDataBackend.Infrastructure.Repositories;

public class UserGameDataRepository : IUserGameDataRepository
{
    private readonly AppDbContext _context;

    public UserGameDataRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserGameData?> GetByUserAndGameAsync(Guid userId, Guid gameId)
    {
        return await _context.UserGameData
            .Include(ugd => ugd.Game)
            .FirstOrDefaultAsync(ugd => ugd.UserId == userId && ugd.GameId == gameId);
    }

    public async Task<List<UserGameData>> GetAllByUserAsync(Guid userId)
    {
        return await _context.UserGameData
            .AsNoTracking()
            .Include(ugd => ugd.Game)
            .Where(ugd => ugd.UserId == userId)
            .OrderByDescending(ugd => ugd.UpdatedAt)
            .ToListAsync();
    }

    public async Task<UserGameData> CreateAsync(UserGameData data)
    {
        _context.UserGameData.Add(data);
        await _context.SaveChangesAsync();
        return data;
    }

    public async Task UpdateAsync(UserGameData data)
    {
        data.UpdatedAt = DateTime.UtcNow;
        _context.UserGameData.Update(data);
        await _context.SaveChangesAsync();
    }

    public async Task<List<UserGameData>> GetTopScoresAsync(Guid gameId, int count)
    {
        return await _context.UserGameData
            .AsNoTracking()
            .Include(ugd => ugd.User)
            .Where(ugd => ugd.GameId == gameId)
            .OrderByDescending(ugd => ugd.Score)
            .Take(count)
            .ToListAsync();
    }

    public async Task<int> GetUserRankAsync(Guid gameId, long userScore)
    {
        var playersAbove = await _context.UserGameData
            .CountAsync(ugd => ugd.GameId == gameId && ugd.Score > userScore);

        return playersAbove + 1;
    }

    public async Task<int> GetTotalPlayersAsync(Guid gameId)
    {
        return await _context.UserGameData
            .CountAsync(ugd => ugd.GameId == gameId);
    }
}
