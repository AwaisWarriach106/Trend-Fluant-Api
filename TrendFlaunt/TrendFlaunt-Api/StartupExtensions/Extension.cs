using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrendFlaunt.Data.Client;
using TrendFlaunt.Data.Interfaces;
using TrendFlaunt.Data.Repository;
using TrendFlaunt.Data.Settings;
using TrendFlaunt.Domain.Authentication;
using TrendFlaunt.Domain.Interfaces;
using TrendFlaunt.Domain.Services;

namespace TrendFlaunt_Api.StartupExtensions;

public static class Extension
{
    public static IServiceCollection AddTrendFlauntConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(configuration["PostgresServer:ConnectionString"]))
            .Configure<PostgresConfiguration>(configuration.GetSection("PostgresServer"));
        var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        services.AddSingleton(emailConfig);
        services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        services
        .AddTransient<ITokenFactory, TokenFactory>()
        .Configure<JwtTokenConfiguration>(configuration.GetSection("JwtTokenSettings"));

        services.AddHttpContextAccessor()
            .AddTransient<IDbClient, DbClient>()
            .AddTransient<IAuthenticationService, AuthenticationService>()
            .AddTransient<IAuthenticationRepository, AuthenticationRepository>()
            .AddTransient<ITokenFactory, TokenFactory>()
            .AddTransient<IEmailService, EmailService>()
            .AddAuthenticationConfiguration(configuration)
            .AddSwaggerConfiguration();
        return services;
    }
}
