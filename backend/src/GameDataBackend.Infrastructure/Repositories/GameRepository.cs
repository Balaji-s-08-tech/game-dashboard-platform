using GameDataBackend.Core.Entities;
using GameDataBackend.Core.Interfaces.Repositories;
using GameDataBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameDataBackend.Infrastructure.Repositories;

public class GameRepository : IGameRepository
{
    private readonly AppDbContext _context;

    public GameRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Game?> GetByIdAsync(Guid id)
    {
        return await _context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Game?> GetByNameAsync(string name)
    {
        return await _context.Games
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Name == name);
    }

    public async Task<List<Game>> GetAllAsync()
    {
        return await _context.Games
            .AsNoTracking()
            .OrderBy(g => g.Name)
            .ToListAsync();
    }

    public async Task<Game> CreateAsync(Game game)
    {
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        return game;
    }
}
