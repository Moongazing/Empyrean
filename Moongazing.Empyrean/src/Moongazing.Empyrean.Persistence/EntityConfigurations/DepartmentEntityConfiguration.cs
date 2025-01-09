using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class DepartmentEntityConfiguration : IEntityTypeConfiguration<DepartmentEntity>
{
    public void Configure(EntityTypeBuilder<DepartmentEntity> builder)
    {
        builder.ToTable("Departments");

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

        builder.HasMany(e => e.Branches)
               .WithOne(e => e.Department)
               .HasForeignKey(e => e.DepartmentId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Employees)
             .WithOne(e => e.Department)
             .HasForeignKey(e => e.DepartmentId)
             .OnDelete(DeleteBehavior.Cascade);

    }
}
