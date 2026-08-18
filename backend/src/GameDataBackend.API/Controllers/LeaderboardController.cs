using System.Security.Claims;
using GameDataBackend.Core.DTOs;
using GameDataBackend.Core.DTOs.Leaderboard;
using GameDataBackend.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameDataBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    public LeaderboardController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    [HttpGet("{gameId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<List<LeaderboardEntryDto>>), StatusCodes.Status200OK)]
    [ResponseCache(Duration = 30)]
    public async Task<IActionResult> GetTopPlayers(Guid gameId, [FromQuery] int top = 10)
    {
        var result = await _leaderboardService.GetTopPlayersAsync(gameId, top);
        return Ok(ApiResponse<List<LeaderboardEntryDto>>.SuccessResponse(result, $"Top {result.Count} players."));
    }

    [HttpGet("{gameId:guid}/rank")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserRankDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserRank(Guid gameId)
    {
        var userId = GetCurrentUserId();
        var result = await _leaderboardService.GetUserRankAsync(userId, gameId);
        return Ok(ApiResponse<UserRankDto>.SuccessResponse(result));
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
