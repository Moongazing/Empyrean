using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class OvertimeEntityConfiguration : IEntityTypeConfiguration<OvertimeEntity>
{
    public void Configure(EntityTypeBuilder<OvertimeEntity> builder)
    {
        builder.ToTable("Overtimes");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("gen_random_uuid()")
               .IsRequired();

        builder.Property(e => e.EmployeeId)
               .IsRequired();

        builder.Property(e => e.StartTime)
               .HasColumnType("timestamp")
               .IsRequired();

        builder.Property(e => e.EndTime)
               .HasColumnType("timestamp")
               .IsRequired();

        builder.Property(e => e.HourlyRate)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(e => e.TotalAmount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.HasOne(e => e.Employee)
               .WithMany()
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
