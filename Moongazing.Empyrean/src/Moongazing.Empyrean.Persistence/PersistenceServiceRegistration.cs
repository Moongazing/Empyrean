using Doing.Retail.Application.Services.Repositories;
using Doing.Retail.Persistence.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Persistence.Contexts;
using Moongazing.Empyrean.Persistence.Repositories;
using Moongazing.Kernel.Persistence.MigrationApplier;
using Polly;
using Policy = Polly.Policy;

namespace Moongazing.Empyrean.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {

        var retryPolicy = Policy
                         .Handle<SqlException>()
                         .WaitAndRetry(
                         [
                            TimeSpan.FromSeconds(10),
                            TimeSpan.FromSeconds(20),
                            TimeSpan.FromSeconds(30),
                         ]);

        services.AddDbContext<BaseDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("EmpyreanDb")));


        services.AddDbMigrationApplier(buildServices => buildServices.GetRequiredService<BaseDbContext>());

        services.AddScoped<IEmailAuthenticatorRepository, EmailAuthenticatorRepository>();
        services.AddScoped<IOperationClaimRepository, OperationClaimRepository>();
        services.AddScoped<IOtpAuthenticatorRepository, OtpAuthenticatorRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();
        services.AddScoped<IAdvanceRequestRepository, AdvanceRequestRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddScoped<IAwardRepository, AwardRepository>();
        services.AddScoped<IBankDetailRepository, BankDetailRepository>();
        services.AddScoped<IBenefitRepository, BenefitRepository>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IDisciplinaryActionRepository, DisciplinaryActionRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IEmergencyContactRepository, EmergencyContactRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IExpenseRecordRepository, ExpenseRecordRepository>();
        services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
        services.AddScoped<IOvertimeRepository, OvertimeRepository>();
        services.AddScoped<IPerformanceReviewRepository, PerformanceReviewRepository>();
        services.AddScoped<IProfessionRepository, ProfessionRepository>();
        services.AddScoped<IPromotionRepository, PromotionRepository>();
        services.AddScoped<IShiftScheduleRepository, ShiftScheduleRepository>();
        services.AddScoped<ITerminationRepository, TerminationRepository>();



        return services;
    }


}