using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class BankDetailEntityConfiguration : IEntityTypeConfiguration<BankDetailEntity>
{
    public void Configure(EntityTypeBuilder<BankDetailEntity> builder)
    {
        builder.ToTable("BankDetails");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("gen_random_uuid()")
               .IsRequired();

        builder.Property(e => e.BankName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.AccountNumber)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.IBAN)
               .HasMaxLength(100)
               .IsRequired();

        builder.HasOne(e => e.Employee)
               .WithOne(e => e.BankDetail)
               .HasForeignKey<BankDetailEntity>(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}