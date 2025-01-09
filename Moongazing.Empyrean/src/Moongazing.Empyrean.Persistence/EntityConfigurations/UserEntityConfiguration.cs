using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Kernel.Persistence.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
               .HasDefaultValueSql("gen_random_uuid()")
               .IsRequired();

        builder.Property(u => u.FirstName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(u => u.LastName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(u => u.Email)
               .HasMaxLength(150)
               .IsRequired();

        builder.Property(u => u.PhoneNumber)
               .HasMaxLength(20)
               .IsRequired(false);

        builder.Property(u => u.PasswordSalt)
               .IsRequired();

        builder.Property(u => u.PasswordHash)
               .IsRequired();

        builder.Property(u => u.AuthenticatorType)
               .HasConversion<string>()
               .IsRequired();

        builder.Property(u => u.Status)
               .HasConversion<string>()
               .IsRequired();

        builder.HasMany(u => u.UserOperationClaims)
               .WithOne(c => c.User)
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.RefreshTokens)
               .WithOne(rt => rt.User)
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.EmailAuthenticators)
               .WithOne(ea => ea.User)
               .HasForeignKey(ea => ea.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.OtpAuthenticators)
               .WithOne(oa => oa.User)
               .HasForeignKey(oa => oa.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
