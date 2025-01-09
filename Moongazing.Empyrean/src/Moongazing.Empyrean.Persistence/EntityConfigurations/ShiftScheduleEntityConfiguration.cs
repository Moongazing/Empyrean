using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class ShiftScheduleEntityConfiguration : IEntityTypeConfiguration<ShiftScheduleEntity>
{
    public void Configure(EntityTypeBuilder<ShiftScheduleEntity> builder)
    {
        builder.ToTable("ShiftSchedules");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("gen_random_uuid()")
               .IsRequired();

        builder.Property(e => e.EmployeeId)
               .IsRequired();

        builder.Property(e => e.ShiftStart)
               .HasColumnType("timestamp")
               .IsRequired();

        builder.Property(e => e.ShiftEnd)
               .HasColumnType("timestamp")
               .IsRequired();

        builder.Property(e => e.Notes)
               .HasMaxLength(500)
               .IsRequired(false);

        builder.HasOne(e => e.Employee)
               .WithMany()
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
