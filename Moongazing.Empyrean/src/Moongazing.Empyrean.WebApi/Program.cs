using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Moongazing.Empyrean.Application;
using Moongazing.Empyrean.Persistence;
using Moongazing.Empyrean.WebApi;
using Moongazing.Kernel.CrossCuttingConcerns.Exceptions.Extensions;
using Moongazing.Kernel.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Moongazing.Kernel.Localization.Middlewares;
using Moongazing.Kernel.Mailing;
using Moongazing.Kernel.Security.Encryption;
using Moongazing.Kernel.Security.Jwt;
using Polly;
using StackExchange.Redis;
using System.Text.Json.Serialization;
using Moongazing.Empyrean.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

#region Configuration and Services Setup

builder.Services.AddApplicationServices(
    postgreSqlConfig: builder.Configuration.GetSection("SeriLogConfigurations:PostgresqlConfiguration").Get<PostgreSqlConfiguration>()!,
    tokenOptions: builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>()!,
    mailSettings: builder.Configuration.GetSection("MailSettings").Get<MailSettings>()!);

builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

const string tokenOptionsConfigurationSection = "TokenOptions";
TokenOptions tokenOptions =
    builder.Configuration.GetSection(tokenOptionsConfigurationSection).Get<TokenOptions>()
    ?? throw new InvalidOperationException($"\"{tokenOptionsConfigurationSection}\" section cannot found in configuration.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
        };
    });

builder.Services.AddDistributedMemoryCache();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(opt =>
    opt.AddDefaultPolicy(p =>
    {
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    })
);

var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
var redisRetryPolicy = Policy
    .Handle<RedisConnectionException>()
    .WaitAndRetry(
    [
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(2),
        TimeSpan.FromSeconds(3)
    ]);

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(redisConnectionString!, true);
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
});

builder.Services.AddOpenApi();

#endregion

var app = builder.Build();

#region Middleware Configuration

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.ConfigureCustomExceptionMiddleware();
}

app.UseHttpsRedirection();
app.UseAuthorization();

const string webApiConfigurationSection = "WebApiConfiguration";
WebApiConfiguration webApiConfiguration =
    app.Configuration.GetSection(webApiConfigurationSection).Get<WebApiConfiguration>()
    ?? throw new InvalidOperationException($"\"{webApiConfigurationSection}\" section cannot found in configuration.");
app.UseCors(opt => opt.WithOrigins(webApiConfiguration.AllowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials());

app.UseResponseLocalization();

#endregion

#region Endpoint Configuration

app.MapControllers();

#endregion

app.Run();
