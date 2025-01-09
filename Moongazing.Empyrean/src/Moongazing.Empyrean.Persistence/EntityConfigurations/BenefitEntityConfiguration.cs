using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class BenefitEntityConfiguration : IEntityTypeConfiguration<BenefitEntity>
{
    public void Configure(EntityTypeBuilder<BenefitEntity> builder)
    {
        builder.ToTable("Benefits");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.Property(e => e.BenefitName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.Description)
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(e => e.Value)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(e => e.EmployeeId)
               .IsRequired();

        builder.HasOne(e => e.Employee)
               .WithMany()
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
