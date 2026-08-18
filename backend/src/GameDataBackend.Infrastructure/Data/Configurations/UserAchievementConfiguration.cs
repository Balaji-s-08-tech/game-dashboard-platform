using GameDataBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameDataBackend.Infrastructure.Data.Configurations;

public class UserAchievementConfiguration : IEntityTypeConfiguration<UserAchievement>
{
    public void Configure(EntityTypeBuilder<UserAchievement> builder)
    {
        builder.ToTable("UserAchievements");
        builder.HasKey(ua => ua.Id);
        builder.Property(ua => ua.Id).HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.HasIndex(ua => new { ua.UserId, ua.AchievementId })
               .IsUnique()
               .HasDatabaseName("IX_UA_UserId_AchievementId");

        builder.HasOne(ua => ua.Achievement)
               .WithMany(a => a.UserAchievements)
               .HasForeignKey(ua => ua.AchievementId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
