using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class UserOperationClaimEntityConfiguration : IEntityTypeConfiguration<UserOperationClaimEntity>
{
    public void Configure(EntityTypeBuilder<UserOperationClaimEntity> builder)
    {
        builder.ToTable("UserOperationClaims");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.Property(e => e.UserId)
               .IsRequired();

        builder.Property(e => e.OperationClaimId)
               .IsRequired();

        builder.HasOne(e => e.User)
               .WithMany(u => u.UserOperationClaims)
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.OperationClaim)
               .WithMany(o => o.UserOperationClaims)
               .HasForeignKey(e => e.OperationClaimId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
