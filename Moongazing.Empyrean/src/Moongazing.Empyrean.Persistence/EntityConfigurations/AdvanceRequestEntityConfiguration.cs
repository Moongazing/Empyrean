using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class AdvanceRequestEntityConfiguration : IEntityTypeConfiguration<AdvanceRequestEntity>
{
    public void Configure(EntityTypeBuilder<AdvanceRequestEntity> builder)
    {
        builder.ToTable("AdvanceRequests");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.Property(e => e.EmployeeId)
               .IsRequired();

        builder.Property(e => e.RequestedAmount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(e => e.ApprovedAmount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(e => e.Reason)
               .HasConversion<string>() 
               .IsRequired();

        builder.Property(e => e.IsApproved)
               .IsRequired();

        builder.Property(e => e.RequestDate)
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(e => e.ApprovalDate)
               .HasColumnType("datetime")
               .IsRequired(false);

        builder.HasOne(e => e.Employee)
               .WithMany()
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
