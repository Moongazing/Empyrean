using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class ProfessionEntityConfiguration : IEntityTypeConfiguration<ProfessionEntity>
{
    public void Configure(EntityTypeBuilder<ProfessionEntity> builder)
    {
        builder.ToTable("Professions");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("gen_random_uuid()")
               .IsRequired();

        builder.Property(e => e.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.Description)
               .HasMaxLength(500)
               .IsRequired();

        builder.HasMany(e => e.Employees)
               .WithOne(e => e.Profession)
               .HasForeignKey(e => e.ProfessionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
