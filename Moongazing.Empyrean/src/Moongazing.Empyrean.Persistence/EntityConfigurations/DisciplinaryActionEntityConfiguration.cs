using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class DisciplinaryActionEntityConfiguration : IEntityTypeConfiguration<DisciplinaryActionEntity>
{
    public void Configure(EntityTypeBuilder<DisciplinaryActionEntity> builder)
    {
        builder.ToTable("DisciplinaryActions");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.Property(e => e.EmployeeId)
               .IsRequired();

        builder.Property(e => e.ActionType)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.Reason)
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(e => e.ActionDate)
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(e => e.Comments)
               .HasMaxLength(1000)
               .IsRequired();

        builder.HasOne(e => e.Employee)
               .WithMany()
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
