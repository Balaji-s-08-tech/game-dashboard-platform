using GameDataBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameDataBackend.Infrastructure.Data.Configurations;

public class UserGameDataConfiguration : IEntityTypeConfiguration<UserGameData>
{
    public void Configure(EntityTypeBuilder<UserGameData> builder)
    {
        builder.ToTable("UserGameData");
        builder.HasKey(ugd => ugd.Id);
        builder.Property(ugd => ugd.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(ugd => ugd.Score).IsRequired();
        builder.Property(ugd => ugd.Level).IsRequired();
        builder.Property(ugd => ugd.ItemsJson).HasMaxLength(4000).HasDefaultValue("[]");

        builder.HasIndex(ugd => new { ugd.UserId, ugd.GameId })
               .HasDatabaseName("IX_UGD_UserId_GameId");

        builder.HasIndex(ugd => new { ugd.GameId, ugd.Score })
               .IsDescending(false, true)
               .HasDatabaseName("IX_UGD_GameId_Score_Desc");

        builder.HasIndex(ugd => ugd.Score)
               .IsDescending(true)
               .HasDatabaseName("IX_UGD_Score_Desc");
    }
}
