using System.ComponentModel.DataAnnotations;

namespace GameDataBackend.Core.DTOs.Game;

public class SaveGameRequestDto
{
    [Required(ErrorMessage = "GameId is required")]
    public Guid GameId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Level must be non-negative")]
    public int Level { get; set; }

    [Range(0, long.MaxValue, ErrorMessage = "Score must be non-negative")]
    public long Score { get; set; }

    public string? ItemsJson { get; set; }

    public bool IsAutoSave { get; set; }
}

public class GameProgressResponseDto
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public string GameName { get; set; } = string.Empty;
    public int Level { get; set; }
    public long Score { get; set; }
    public string ItemsJson { get; set; } = "[]";
    public bool IsAutoSave { get; set; }
    public DateTime SavedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
