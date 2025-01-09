using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class AwardEntityConfiguration : IEntityTypeConfiguration<AwardEntity>
{
    public void Configure(EntityTypeBuilder<AwardEntity> builder)
    {
        builder.ToTable("Awards");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("gen_random_uuid()")
               .IsRequired();

        builder.Property(e => e.AwardName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.Description)
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(e => e.AwardDate)
               .HasColumnType("timestamp")
               .IsRequired();

        builder.Property(e => e.EmployeeId)
               .IsRequired();

        builder.HasOne(e => e.Employee)
               .WithMany()
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
