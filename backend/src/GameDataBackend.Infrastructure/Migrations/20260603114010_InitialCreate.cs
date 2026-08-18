using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace GameDataBackend.Infrastructure.Migrations
{

    public partial class InitialCreate : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ConditionType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ConditionValue = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Achievements_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LoginTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LogoutTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PlayDurationMinutes = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGameData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<long>(type: "INTEGER", nullable: false),
                    ItemsJson = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false, defaultValue: "[]"),
                    IsAutoSave = table.Column<bool>(type: "INTEGER", nullable: false),
                    SavedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGameData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGameData_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGameData_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAchievements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AchievementId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UnlockedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAchievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAchievements_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "Achievements",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAchievements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Epic RPG adventure game", "Dragon Quest" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Arcade space shooter", "Space Blaster" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Brain-teasing puzzle game", "Puzzle Master" }
                });

            migrationBuilder.InsertData(
                table: "Achievements",
                columns: new[] { "Id", "ConditionType", "ConditionValue", "CreatedAt", "Description", "GameId", "Name" },
                values: new object[,]
                {
                    { new Guid("aaaa0001-0000-0000-0000-000000000001"), "level_gte", 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Reach level 5", new Guid("11111111-1111-1111-1111-111111111111"), "First Steps" },
                    { new Guid("aaaa0002-0000-0000-0000-000000000002"), "level_gte", 25, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Reach level 25", new Guid("11111111-1111-1111-1111-111111111111"), "Veteran Warrior" },
                    { new Guid("aaaa0003-0000-0000-0000-000000000003"), "score_gte", 10000, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Score 10,000 points", new Guid("11111111-1111-1111-1111-111111111111"), "Score Hunter" },
                    { new Guid("aaaa0004-0000-0000-0000-000000000004"), "score_gte", 100000, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Score 100,000 points", new Guid("11111111-1111-1111-1111-111111111111"), "Legend" },
                    { new Guid("bbbb0001-0000-0000-0000-000000000001"), "level_gte", 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Reach level 3", new Guid("22222222-2222-2222-2222-222222222222"), "Cadet" },
                    { new Guid("bbbb0002-0000-0000-0000-000000000002"), "score_gte", 50000, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Score 50,000 points", new Guid("22222222-2222-2222-2222-222222222222"), "Ace Pilot" },
                    { new Guid("cccc0001-0000-0000-0000-000000000001"), "level_gte", 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Reach level 10", new Guid("33333333-3333-3333-3333-333333333333"), "Quick Thinker" },
                    { new Guid("cccc0002-0000-0000-0000-000000000002"), "score_gte", 25000, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Score 25,000 points", new Guid("33333333-3333-3333-3333-333333333333"), "Puzzle Genius" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_GameId",
                table: "Achievements",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_Name",
                table: "Games",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UA_UserId_AchievementId",
                table: "UserAchievements",
                columns: new[] { "UserId", "AchievementId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAchievements_AchievementId",
                table: "UserAchievements",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_UGD_GameId_Score_Desc",
                table: "UserGameData",
                columns: new[] { "GameId", "Score" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_UGD_Score_Desc",
                table: "UserGameData",
                column: "Score",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_UGD_UserId_GameId",
                table: "UserGameData",
                columns: new[] { "UserId", "GameId" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "UserAchievements");

            migrationBuilder.DropTable(
                name: "UserGameData");

            migrationBuilder.DropTable(
                name: "Achievements");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
