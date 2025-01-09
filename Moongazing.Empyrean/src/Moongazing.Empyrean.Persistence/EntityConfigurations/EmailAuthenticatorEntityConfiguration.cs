using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class EmailAuthenticatorEntityConfiguration : IEntityTypeConfiguration<EmailAuthenticatorEntity>
{
    public void Configure(EntityTypeBuilder<EmailAuthenticatorEntity> builder)
    {
        builder.ToTable("EmailAuthenticators");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.Property(e => e.UserId)
               .IsRequired();

        builder.Property(e => e.ActivationKey)
               .HasMaxLength(256)
               .IsRequired(false);

        builder.Property(e => e.IsVerified)
               .IsRequired();

        builder.HasOne(e => e.User)
               .WithMany(u => u.EmailAuthenticators)
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
