using System.Security.Claims;
using GameDataBackend.Core.DTOs;
using GameDataBackend.Core.DTOs.Game;
using GameDataBackend.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameDataBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpPost("save")]
    [ProducesResponseType(typeof(ApiResponse<GameProgressResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SaveProgress([FromBody] SaveGameRequestDto request)
    {
        var userId = GetCurrentUserId();
        var result = await _gameService.SaveProgressAsync(userId, request);
        return Ok(ApiResponse<GameProgressResponseDto>.SuccessResponse(result, "Game progress saved successfully."));
    }

    [HttpGet("progress/{gameId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GameProgressResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LoadProgress(Guid gameId)
    {
        var userId = GetCurrentUserId();
        var result = await _gameService.LoadProgressAsync(userId, gameId);

        if (result == null)
        {
            return NotFound(ApiResponse<object>.FailResponse("No game progress found for this game."));
        }

        return Ok(ApiResponse<GameProgressResponseDto>.SuccessResponse(result));
    }

    [HttpGet("progress")]
    [ProducesResponseType(typeof(ApiResponse<List<GameProgressResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoadAllProgress()
    {
        var userId = GetCurrentUserId();
        var result = await _gameService.LoadAllProgressAsync(userId);
        return Ok(ApiResponse<List<GameProgressResponseDto>>.SuccessResponse(result));
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
