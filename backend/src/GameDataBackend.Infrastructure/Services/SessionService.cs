using GameDataBackend.Core.DTOs.Session;
using GameDataBackend.Core.Entities;
using GameDataBackend.Core.Exceptions;
using GameDataBackend.Core.Interfaces.Repositories;
using GameDataBackend.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace GameDataBackend.Infrastructure.Services;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ILogger<SessionService> _logger;

    public SessionService(ISessionRepository sessionRepository, ILogger<SessionService> logger)
    {
        _sessionRepository = sessionRepository;
        _logger = logger;
    }

    public async Task<SessionResponseDto> StartSessionAsync(Guid userId)
    {
        _logger.LogInformation("Starting session for user {UserId}", userId);

        var activeSession = await _sessionRepository.GetActiveSessionAsync(userId);
        if (activeSession != null)
        {
            _logger.LogWarning("Auto-closing previous active session {SessionId} for user {UserId}", activeSession.Id, userId);
            activeSession.LogoutTime = DateTime.UtcNow;
            activeSession.PlayDurationMinutes = (activeSession.LogoutTime.Value - activeSession.LoginTime).TotalMinutes;
            await _sessionRepository.UpdateAsync(activeSession);
        }

        var session = new Session
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            LoginTime = DateTime.UtcNow
        };

        await _sessionRepository.CreateAsync(session);

        _logger.LogInformation("Session started: {SessionId} for user {UserId}", session.Id, userId);

        return MapToResponse(session);
    }

    public async Task<SessionResponseDto> EndSessionAsync(Guid userId, Guid sessionId)
    {
        _logger.LogInformation("Ending session {SessionId} for user {UserId}", sessionId, userId);

        var session = await _sessionRepository.GetByIdAsync(sessionId)
            ?? throw new NotFoundException("Session", sessionId);

        if (session.UserId != userId)
        {
            throw new UnauthorizedException("You do not have permission to end this session.");
        }

        if (session.LogoutTime != null)
        {
            throw new BadRequestException("This session has already been ended.");
        }

        session.LogoutTime = DateTime.UtcNow;
        session.PlayDurationMinutes = Math.Round((session.LogoutTime.Value - session.LoginTime).TotalMinutes, 2);

        await _sessionRepository.UpdateAsync(session);

        _logger.LogInformation("Session ended: {SessionId}, duration: {Duration} minutes", session.Id, session.PlayDurationMinutes);

        return MapToResponse(session);
    }

    public async Task<SessionHistoryResponseDto> GetSessionHistoryAsync(Guid userId)
    {
        _logger.LogInformation("Fetching session history for user {UserId}", userId);

        var sessions = await _sessionRepository.GetByUserIdAsync(userId);

        var sessionDtos = sessions.Select(MapToResponse).ToList();
        var totalPlayTime = sessions
            .Where(s => s.PlayDurationMinutes.HasValue)
            .Sum(s => s.PlayDurationMinutes!.Value);

        return new SessionHistoryResponseDto
        {
            Sessions = sessionDtos,
            TotalPlayTimeMinutes = Math.Round(totalPlayTime, 2),
            TotalSessions = sessions.Count
        };
    }

    private static SessionResponseDto MapToResponse(Session session)
    {
        return new SessionResponseDto
        {
            Id = session.Id,
            LoginTime = session.LoginTime,
            LogoutTime = session.LogoutTime,
            PlayDurationMinutes = session.PlayDurationMinutes
        };
    }
}
