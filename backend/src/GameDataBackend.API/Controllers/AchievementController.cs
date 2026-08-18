using System.Security.Claims;
using GameDataBackend.Core.DTOs;
using GameDataBackend.Core.DTOs.Achievement;
using GameDataBackend.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameDataBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AchievementController : ControllerBase
{
    private readonly IAchievementService _achievementService;

    public AchievementController(IAchievementService achievementService)
    {
        _achievementService = achievementService;
    }

    [HttpPost("check")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<AchievementUnlockResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckAndUnlock([FromBody] CheckAchievementsRequestDto request)
    {
        var userId = GetCurrentUserId();
        var result = await _achievementService.CheckAndUnlockAsync(userId, request.GameId);

        var message = result.NewlyUnlocked > 0
            ? $"{result.NewlyUnlocked} new achievement(s) unlocked!"
            : "No new achievements unlocked.";

        return Ok(ApiResponse<AchievementUnlockResultDto>.SuccessResponse(result, message));
    }

    [HttpGet("user")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<List<UserAchievementDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserAchievements()
    {
        var userId = GetCurrentUserId();
        var result = await _achievementService.GetUserAchievementsAsync(userId);
        return Ok(ApiResponse<List<UserAchievementDto>>.SuccessResponse(result));
    }

    [HttpGet("game/{gameId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<List<AchievementDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGameAchievements(Guid gameId)
    {
        var result = await _achievementService.GetGameAchievementsAsync(gameId);
        return Ok(ApiResponse<List<AchievementDto>>.SuccessResponse(result));
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
