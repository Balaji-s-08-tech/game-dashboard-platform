using GameDataBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameDataBackend.Infrastructure.Data.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Id).HasDefaultValueSql("NEWSEQUENTIALID()");
        builder.Property(g => g.Name).IsRequired().HasMaxLength(100);
        builder.Property(g => g.Description).HasMaxLength(500);

        builder.HasIndex(g => g.Name).IsUnique().HasDatabaseName("IX_Games_Name");

        builder.HasMany(g => g.UserGameData)
               .WithOne(ugd => ugd.Game)
               .HasForeignKey(ugd => ugd.GameId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Achievements)
               .WithOne(a => a.Game)
               .HasForeignKey(a => a.GameId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
