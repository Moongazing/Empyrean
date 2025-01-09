﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Security.Models;
using System.Reflection;

namespace Moongazing.Empyrean.Persistence.Contexts;

public class BaseDbContext : DbContext
{
    protected IHttpContextAccessor HttpContextAccessor;
    protected IConfiguration Configuration { get; set; }
    public virtual DbSet<UserEntity> Users { get; set; }
    public virtual DbSet<OperationClaimEntity> OperationClaims { get; set; }
    public virtual DbSet<UserOperationClaimEntity> UserOperationClaims { get; set; }
    public virtual DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public virtual DbSet<EmailAuthenticatorEntity> EmailAuthenticators { get; set; }
    public virtual DbSet<OtpAuthenticatorEntity> OtpAuthenticators { get; set; }
    public virtual DbSet<EmployeeEntity> Employees { get; set; }
    public virtual DbSet<AdvanceRequestEntity> AdvanceRequests { get; set; }
    public virtual DbSet<AttendanceEntity> Attendances { get; set; }
    public virtual DbSet<AwardEntity> Awards { get; set; }
    public virtual DbSet<BankDetailEntity> BankDetails { get; set; }
    public virtual DbSet<BenefitEntity> Benefits { get; set; }
    public virtual DbSet<BranchEntity> Branches { get; set; }
    public virtual DbSet<DepartmentEntity> Departments { get; set; }
    public virtual DbSet<DisciplinaryActionEntity> DisciplinaryActions { get; set; }
    public virtual DbSet<DocumentEntity> Documents { get; set; }
    public virtual DbSet<EmergencyContactEntity> EmergencyContacts { get; set; }
    public virtual DbSet<ExpenseRecordEntity> ExpenseRecords { get; set; }
    public virtual DbSet<LeaveRequestEntity> LeaveRequests { get; set; }
    public virtual DbSet<OvertimeEntity> Overtimes { get; set; }
    public virtual DbSet<PerformanceReviewEntity> PerformanceReviews { get; set; }
    public virtual DbSet<PromotionEntity> Promotions { get; set; }
    public virtual DbSet<ShiftScheduleEntity> ShiftSchedules { get; set; }
    public virtual DbSet<TerminationEntity> Terminations { get; set; }
    public virtual DbSet<PayrollEntity> Payrolls { get; set; }


    public BaseDbContext(DbContextOptions<BaseDbContext> options, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        HttpContextAccessor = httpContextAccessor;
        Configuration = configuration;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

}
