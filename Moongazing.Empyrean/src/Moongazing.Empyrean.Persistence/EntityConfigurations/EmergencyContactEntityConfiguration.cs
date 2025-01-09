using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class EmergencyContactEntityConfiguration : IEntityTypeConfiguration<EmergencyContactEntity>
{
    public void Configure(EntityTypeBuilder<EmergencyContactEntity> builder)
    {
        builder.ToTable("EmergencyContacts");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.Property(e => e.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.PhoneNumber)
               .HasMaxLength(15)
               .IsRequired();

        builder.Property(e => e.Relation)
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(e => e.EmployeeId)
               .IsRequired();

        builder.HasOne(e => e.Employee)
               .WithMany()
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
