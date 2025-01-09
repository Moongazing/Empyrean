using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class PromotionEntityConfiguration : IEntityTypeConfiguration<PromotionEntity>
{
    public void Configure(EntityTypeBuilder<PromotionEntity> builder)
    {
        builder.ToTable("Promotions");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.Property(e => e.EmployeeId)
               .IsRequired();

        builder.Property(e => e.NewPosition)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.PromotionDate)
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(e => e.ApprovedBy)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.Comments)
               .HasMaxLength(500)
               .IsRequired();

        builder.HasOne(e => e.Employee)
               .WithMany()
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
