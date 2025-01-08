using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moongazing.Empyrean.Persistence.Contexts;
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

        services.AddHttpContextAccessor();

        services.AddDbMigrationApplier(buildServices => buildServices.GetRequiredService<BaseDbContext>());



        return services;
    }


}