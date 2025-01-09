using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class TerminationEntityConfiguration : IEntityTypeConfiguration<TerminationEntity>
{
    public void Configure(EntityTypeBuilder<TerminationEntity> builder)
    {
        builder.ToTable("Terminations");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.Property(e => e.EmployeeId)
               .IsRequired();

        builder.Property(e => e.Reason)
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(e => e.TerminationDate)
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(e => e.Comments)
               .HasMaxLength(1000)
               .IsRequired(false);

        builder.Property(e => e.IsVoluntary)
               .IsRequired();

        builder.HasOne(e => e.Employee)
               .WithMany()
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
