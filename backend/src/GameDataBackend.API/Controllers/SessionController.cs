using System.Security.Claims;
using GameDataBackend.Core.DTOs;
using GameDataBackend.Core.DTOs.Session;
using GameDataBackend.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameDataBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost("start")]
    [ProducesResponseType(typeof(ApiResponse<SessionResponseDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> StartSession()
    {
        var userId = GetCurrentUserId();
        var result = await _sessionService.StartSessionAsync(userId);
        return StatusCode(StatusCodes.Status201Created,
            ApiResponse<SessionResponseDto>.SuccessResponse(result, "Session started."));
    }

    [HttpPut("end/{sessionId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<SessionResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> EndSession(Guid sessionId)
    {
        var userId = GetCurrentUserId();
        var result = await _sessionService.EndSessionAsync(userId, sessionId);
        return Ok(ApiResponse<SessionResponseDto>.SuccessResponse(result, "Session ended."));
    }

    [HttpGet("history")]
    [ProducesResponseType(typeof(ApiResponse<SessionHistoryResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory()
    {
        var userId = GetCurrentUserId();
        var result = await _sessionService.GetSessionHistoryAsync(userId);
        return Ok(ApiResponse<SessionHistoryResponseDto>.SuccessResponse(result));
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("Invalid user token.");

        return userId;
    }
}
