using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class PayrollEntityConfiguration : IEntityTypeConfiguration<PayrollEntity>
{
    public void Configure(EntityTypeBuilder<PayrollEntity> builder)
    {
        builder.ToTable("Payrolls");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.Property(e => e.EmployeeId)
               .IsRequired();

        builder.Property(e => e.PayDate)
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(e => e.GrossSalary)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(e => e.NetSalary)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(e => e.TaxDeduction)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(e => e.Bonus)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.HasOne(e => e.Employee)
               .WithMany(e => e.Payrolls) // EmployeeEntity ile ilişkisi
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
