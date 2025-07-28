using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrendFlaunt.Data.Client;
using TrendFlaunt.Data.Interfaces;
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
        services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        services
        .AddTransient<ITokenFactory, TokenFactory>()
        .Configure<JwtTokenConfiguration>(configuration.GetSection("JwtTokenSettings"));

        services.AddHttpContextAccessor()
            .AddTransient<IDbClient, DbClient>()
            .AddTransient<IAuthenticationService, AuthenticationService>()
            .AddTransient<ITokenFactory, TokenFactory>()
            .AddSwaggerConfiguration();
        return services;
    }
}
