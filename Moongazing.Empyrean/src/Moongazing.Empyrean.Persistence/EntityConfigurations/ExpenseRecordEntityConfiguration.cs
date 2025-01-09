using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class ExpenseRecordEntityConfiguration : IEntityTypeConfiguration<ExpenseRecordEntity>
{
    public void Configure(EntityTypeBuilder<ExpenseRecordEntity> builder)
    {
        builder.ToTable("ExpenseRecords");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.Property(e => e.EmployeeId)
               .IsRequired();

        builder.Property(e => e.ExpenseType)
               .HasConversion<string>() 
               .IsRequired();

        builder.Property(e => e.Amount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(e => e.Description)
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(e => e.ExpenseDate)
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(e => e.IsReimbursed)
               .IsRequired();

        builder.HasOne(e => e.Employee)
               .WithMany()
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
