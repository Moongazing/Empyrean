using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class LeaveRequestEntityConfiguration : IEntityTypeConfiguration<LeaveRequestEntity>
{
    public void Configure(EntityTypeBuilder<LeaveRequestEntity> builder)
    {
        builder.ToTable("LeaveRequests");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.Property(e => e.EmployeeId)
               .IsRequired();

        builder.Property(e => e.StartDate)
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(e => e.EndDate)
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(e => e.LeaveType)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(e => e.Reason)
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(e => e.IsApproved)
               .IsRequired();

        builder.Property(e => e.RequestDate)
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(e => e.ApprovedBy)
               .HasMaxLength(100)
               .IsRequired();

        builder.HasOne(e => e.Employee)
               .WithMany()
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
