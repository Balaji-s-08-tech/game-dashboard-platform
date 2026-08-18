
using System;
using GameDataBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GameDataBackend.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.27");

            modelBuilder.Entity("GameDataBackend.Core.Entities.Achievement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<string>("ConditionType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("ConditionValue")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GameId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId")
                        .HasDatabaseName("IX_Achievements_GameId");

                    b.ToTable("Achievements", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("aaaa0001-0000-0000-0000-000000000001"),
                            ConditionType = "level_gte",
                            ConditionValue = 5,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Reach level 5",
                            GameId = new Guid("11111111-1111-1111-1111-111111111111"),
                            Name = "First Steps"
                        },
                        new
                        {
                            Id = new Guid("aaaa0002-0000-0000-0000-000000000002"),
                            ConditionType = "level_gte",
                            ConditionValue = 25,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Reach level 25",
                            GameId = new Guid("11111111-1111-1111-1111-111111111111"),
                            Name = "Veteran Warrior"
                        },
                        new
                        {
                            Id = new Guid("aaaa0003-0000-0000-0000-000000000003"),
                            ConditionType = "score_gte",
                            ConditionValue = 10000,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Score 10,000 points",
                            GameId = new Guid("11111111-1111-1111-1111-111111111111"),
                            Name = "Score Hunter"
                        },
                        new
                        {
                            Id = new Guid("aaaa0004-0000-0000-0000-000000000004"),
                            ConditionType = "score_gte",
                            ConditionValue = 100000,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Score 100,000 points",
                            GameId = new Guid("11111111-1111-1111-1111-111111111111"),
                            Name = "Legend"
                        },
                        new
                        {
                            Id = new Guid("bbbb0001-0000-0000-0000-000000000001"),
                            ConditionType = "level_gte",
                            ConditionValue = 3,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Reach level 3",
                            GameId = new Guid("22222222-2222-2222-2222-222222222222"),
                            Name = "Cadet"
                        },
                        new
                        {
                            Id = new Guid("bbbb0002-0000-0000-0000-000000000002"),
                            ConditionType = "score_gte",
                            ConditionValue = 50000,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Score 50,000 points",
                            GameId = new Guid("22222222-2222-2222-2222-222222222222"),
                            Name = "Ace Pilot"
                        },
                        new
                        {
                            Id = new Guid("cccc0001-0000-0000-0000-000000000001"),
                            ConditionType = "level_gte",
                            ConditionValue = 10,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Reach level 10",
                            GameId = new Guid("33333333-3333-3333-3333-333333333333"),
                            Name = "Quick Thinker"
                        },
                        new
                        {
                            Id = new Guid("cccc0002-0000-0000-0000-000000000002"),
                            ConditionType = "score_gte",
                            ConditionValue = 25000,
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Score 25,000 points",
                            GameId = new Guid("33333333-3333-3333-3333-333333333333"),
                            Name = "Puzzle Genius"
                        });
                });

            modelBuilder.Entity("GameDataBackend.Core.Entities.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("IX_Games_Name");

                    b.ToTable("Games", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("11111111-1111-1111-1111-111111111111"),
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Epic RPG adventure game",
                            Name = "Dragon Quest"
                        },
                        new
                        {
                            Id = new Guid("22222222-2222-2222-2222-222222222222"),
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Arcade space shooter",
                            Name = "Space Blaster"
                        },
                        new
                        {
                            Id = new Guid("33333333-3333-3333-3333-333333333333"),
                            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Description = "Brain-teasing puzzle game",
                            Name = "Puzzle Master"
                        });
                });

            modelBuilder.Entity("GameDataBackend.Core.Entities.Session", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<DateTime>("LoginTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LogoutTime")
                        .HasColumnType("TEXT");

                    b.Property<double?>("PlayDurationMinutes")
                        .HasColumnType("REAL");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("IX_Sessions_UserId");

                    b.ToTable("Sessions", (string)null);
                });

            modelBuilder.Entity("GameDataBackend.Core.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("IX_Users_Email");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("GameDataBackend.Core.Entities.UserAchievement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<Guid>("AchievementId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UnlockedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AchievementId");

                    b.HasIndex("UserId", "AchievementId")
                        .IsUnique()
                        .HasDatabaseName("IX_UA_UserId_AchievementId");

                    b.ToTable("UserAchievements", (string)null);
                });

            modelBuilder.Entity("GameDataBackend.Core.Entities.UserGameData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<Guid>("GameId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAutoSave")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ItemsJson")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(4000)
                        .HasColumnType("TEXT")
                        .HasDefaultValue("[]");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("SavedAt")
                        .HasColumnType("TEXT");

                    b.Property<long>("Score")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Score")
                        .IsDescending()
                        .HasDatabaseName("IX_UGD_Score_Desc");

                    b.HasIndex("GameId", "Score")
                        .IsDescending(false, true)
                        .HasDatabaseName("IX_UGD_GameId_Score_Desc");

                    b.HasIndex("UserId", "GameId")
                        .HasDatabaseName("IX_UGD_UserId_GameId");

                    b.ToTable("UserGameData", (string)null);
                });

            modelBuilder.Entity("GameDataBackend.Core.Entities.Achievement", b =>
                {
                    b.HasOne("GameDataBackend.Core.Entities.Game", "Game")
                        .WithMany("Achievements")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("GameDataBackend.Core.Entities.Session", b =>
                {
                    b.HasOne("GameDataBackend.Core.Entities.User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GameDataBackend.Core.Entities.UserAchievement", b =>
                {
                    b.HasOne("GameDataBackend.Core.Entities.Achievement", "Achievement")
                        .WithMany("UserAchievements")
                        .HasForeignKey("AchievementId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("GameDataBackend.Core.Entities.User", "User")
                        .WithMany("UserAchievements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Achievement");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GameDataBackend.Core.Entities.UserGameData", b =>
                {
                    b.HasOne("GameDataBackend.Core.Entities.Game", "Game")
                        .WithMany("UserGameData")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameDataBackend.Core.Entities.User", "User")
                        .WithMany("GameData")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GameDataBackend.Core.Entities.Achievement", b =>
                {
                    b.Navigation("UserAchievements");
                });

            modelBuilder.Entity("GameDataBackend.Core.Entities.Game", b =>
                {
                    b.Navigation("Achievements");

                    b.Navigation("UserGameData");
                });

            modelBuilder.Entity("GameDataBackend.Core.Entities.User", b =>
                {
                    b.Navigation("GameData");

                    b.Navigation("Sessions");

                    b.Navigation("UserAchievements");
                });
#pragma warning restore 612, 618
        }
    }
}
