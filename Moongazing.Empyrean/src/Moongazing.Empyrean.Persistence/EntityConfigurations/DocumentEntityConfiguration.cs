using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Infrastructure.Configurations;

public class DocumentEntityConfiguration : IEntityTypeConfiguration<DocumentEntity>
{
    public void Configure(EntityTypeBuilder<DocumentEntity> builder)
    {
        builder.ToTable("Documents");

        builder.HasKey(d => d.Id);

        builder.Property(e => e.Id)
              .HasDefaultValueSql("gen_random_uuid()")
              .IsRequired();

        builder.Property(e => e.Type)
              .HasConversion<string>()
              .IsRequired();

        builder.Property(d => d.DocumentName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(d => d.DocumentUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(d => d.UploadedAt)
            .IsRequired();

        builder.Property(d => d.EmployeeId)
            .IsRequired();


        builder.HasOne(d => d.Employee)
            .WithMany(e => e.Documents)
            .HasForeignKey(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(d => d.DocumentName).IsUnique(false);
        builder.HasIndex(d => d.EmployeeId);
    }
}
