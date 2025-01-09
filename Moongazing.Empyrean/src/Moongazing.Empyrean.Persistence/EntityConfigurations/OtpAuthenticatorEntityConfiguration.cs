using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class OtpAuthenticatorEntityConfiguration : IEntityTypeConfiguration<OtpAuthenticatorEntity>
{
    public void Configure(EntityTypeBuilder<OtpAuthenticatorEntity> builder)
    {
        builder.ToTable("OtpAuthenticators");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.Property(e => e.UserId)
               .IsRequired();

        builder.Property(e => e.SecretKey)
               .IsRequired();

        builder.Property(e => e.IsVerified)
               .IsRequired();

        builder.HasOne(e => e.User)
               .WithMany(u => u.OtpAuthenticators)
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
