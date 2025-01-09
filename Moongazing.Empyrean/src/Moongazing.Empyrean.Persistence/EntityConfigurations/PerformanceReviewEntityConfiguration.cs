using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class PerformanceReviewEntityConfiguration : IEntityTypeConfiguration<PerformanceReviewEntity>
{
    public void Configure(EntityTypeBuilder<PerformanceReviewEntity> builder)
    {
        builder.ToTable("PerformanceReviews");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("gen_random_uuid()")
               .IsRequired();

        builder.Property(e => e.EmployeeId)
               .IsRequired();

        builder.Property(e => e.ReviewDate)
               .HasColumnType("timestamp")
               .IsRequired();

        builder.Property(e => e.Reviewer)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.Score)
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
