using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class OperationClaimEntityConfiguration : IEntityTypeConfiguration<OperationClaimEntity>
{
    public void Configure(EntityTypeBuilder<OperationClaimEntity> builder)
    {
        builder.ToTable("OperationClaims");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.Property(e => e.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.HasMany(e => e.UserOperationClaims)
               .WithOne(u => u.OperationClaim)
               .HasForeignKey(u => u.OperationClaimId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
