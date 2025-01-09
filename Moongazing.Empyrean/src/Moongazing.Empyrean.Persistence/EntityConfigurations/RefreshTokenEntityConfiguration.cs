using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .ValueGeneratedOnAdd()
               .IsRequired();

        builder.Property(e => e.UserId)
               .IsRequired();

        builder.Property(e => e.Token)
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(e => e.Expires)
               .HasColumnType("timestamp")
               .IsRequired();

        builder.Property(e => e.CreatedByIp)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(e => e.Revoked)
               .HasColumnType("timestamp")
               .IsRequired(false);

        builder.Property(e => e.RevokedByIp)
               .HasMaxLength(50)
               .IsRequired(false);

        builder.Property(e => e.ReplacedByToken)
               .HasMaxLength(500)
               .IsRequired(false);

        builder.Property(e => e.ReasonRevoked)
               .HasMaxLength(250)
               .IsRequired(false);

        builder.HasOne(e => e.User)
               .WithMany(u => u.RefreshTokens)
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
