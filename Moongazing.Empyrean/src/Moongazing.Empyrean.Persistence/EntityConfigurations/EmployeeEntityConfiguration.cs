using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moongazing.Empyrean.Domain.Entities;

namespace Moongazing.Empyrean.Persistence.EntityConfigurations;

public class EmployeeEntityConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasDefaultValueSql("NEWID()")
               .IsRequired();

        builder.Property(e => e.FirstName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.LastName)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(e => e.Email)
               .HasMaxLength(150)
               .IsRequired();

        builder.Property(e => e.PhoneNumber)
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(e => e.DateOfBirth)
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(e => e.Address)
               .HasMaxLength(250)
               .IsRequired();

        builder.Property(e => e.ProfilePictureUrl)
               .HasMaxLength(250)
               .IsRequired();

        builder.Property(e => e.Status)
               .HasConversion<string>()
               .IsRequired();

        builder.Property(e => e.HireDate)
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(e => e.DepartmentId)
               .IsRequired();

        builder.Property(e => e.ProfessionId)
               .IsRequired();

        builder.Property(e => e.ManagerId)
               .IsRequired();

        builder.Property(e => e.BranchId)
               .IsRequired();

        builder.Property(e => e.BankId)
               .IsRequired();

        builder.Property(e => e.TerminationId)
               .IsRequired(false);

        builder.Property(e => e.GrossSalary)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(e => e.NetSalary)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(e => e.TaxAmount)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.Property(e => e.Bonus)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.HasOne(e => e.TerminationDetails)
               .WithOne()
               .HasForeignKey<EmployeeEntity>(e => e.TerminationId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.BankDetail)
               .WithOne()
               .HasForeignKey<EmployeeEntity>(e => e.BankId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Branch)
               .WithMany()
               .HasForeignKey(e => e.BranchId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Manager)
               .WithMany(e => e.Subordinates)
               .HasForeignKey(e => e.ManagerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Profession)
               .WithMany(e => e.Employees)
               .HasForeignKey(e => e.ProfessionId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Department)
               .WithMany()
               .HasForeignKey(e => e.DepartmentId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.LeaveRequests)
               .WithOne(e => e.Employee)
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.AdvanceRequests)
               .WithOne(e => e.Employee)
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.ExpenseRecords)
               .WithOne(e => e.Employee)
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Overtimes)
               .WithOne(e => e.Employee)
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.PerformanceReviews)
               .WithOne(e => e.Employee)
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.EmergencyContacts)
               .WithOne(e => e.Employee)
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Documents)
               .WithOne()
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.ShiftSchedules)
               .WithOne(e => e.Employee)
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Benefits)
               .WithOne(e => e.Employee)
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Promotions)
               .WithOne(e => e.Employee)
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.DisciplinaryActions)
               .WithOne(e => e.Employee)
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Payrolls)
              .WithOne(e => e.Employee)
              .HasForeignKey(e => e.EmployeeId)
              .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Awards)
               .WithOne(e => e.Employee)
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Attendances)
               .WithOne(e => e.Employee)
               .HasForeignKey(e => e.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
