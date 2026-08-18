using GameDataBackend.Core.Entities;
using GameDataBackend.Core.Interfaces.Repositories;
using GameDataBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameDataBackend.Infrastructure.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _context;

    public SessionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Session?> GetByIdAsync(Guid id)
    {
        return await _context.Sessions
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Session>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Sessions
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.LoginTime)
            .ToListAsync();
    }

    public async Task<Session> CreateAsync(Session session)
    {
        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task UpdateAsync(Session session)
    {
        _context.Sessions.Update(session);
        await _context.SaveChangesAsync();
    }

    public async Task<Session?> GetActiveSessionAsync(Guid userId)
    {
        return await _context.Sessions
            .FirstOrDefaultAsync(s => s.UserId == userId && s.LogoutTime == null);
    }
}
