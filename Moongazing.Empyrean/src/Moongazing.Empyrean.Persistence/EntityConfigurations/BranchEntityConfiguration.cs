using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class BranchEntityConfiguration : IEntityTypeConfiguration<BranchEntity>
{
    public void Configure(EntityTypeBuilder<BranchEntity> builder)
    {
        builder.ToTable("Branches");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("gen_random_uuid()")
               .IsRequired();

        builder.Property(e => e.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.Address)
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(e => e.DepartmentId)
               .IsRequired();

        builder.Property(e => e.PhoneNumber)
               .HasMaxLength(20)
               .IsRequired();

        builder.HasOne(e => e.Department)
               .WithMany(e => e.Branches)
               .HasForeignKey(e => e.DepartmentId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Employees)
               .WithOne(e => e.Branch)
               .HasForeignKey(e => e.BranchId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}