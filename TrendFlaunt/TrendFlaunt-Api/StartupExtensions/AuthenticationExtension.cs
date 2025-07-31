using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;
using TrendFlaunt.Data.Settings;
using TrendFlaunt.Domain.Authentication;

namespace TrendFlaunt_Api.StartupExtensions;

public static class AuthenticationExtension
{
    public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services
        .AddTransient<ITokenFactory, TokenFactory>()
        .Configure<JwtTokenConfiguration>(configuration.GetSection("JwtTokenSettings"));

        services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = "Bearer";
            config.DefaultChallengeScheme = "Bearer";
        })
        .AddJwtBearer(config =>
        {
            config = GetBearerOptions(config, configuration.GetValue<string>("JwtTokenSettings:TokenSecret"));

        })
        .AddGoogle("Google", googleOptions =>
        {
            googleOptions.ClientId = configuration["GoogleAuth:ClientId"];
            googleOptions.ClientSecret = configuration["GoogleAuth:ClientSecret"];
        });
        return services;
    }
    private static JwtBearerOptions GetBearerOptions(JwtBearerOptions config, string tokenSecret)
    {
        config.RequireHttpsMetadata = false;
        config.SaveToken = true;
        config.TokenValidationParameters = new TokenValidationParameters()
        {
            RequireExpirationTime = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.FromSeconds(0),
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(tokenSecret))
        };
        config.Events = new JwtBearerEvents()
        {
            OnTokenValidated = context =>
            {
                try
                {
                    var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                    var userIdClaim = context.Principal.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;
                    var profileIdClaim = context.Principal.Claims.FirstOrDefault(x => x.Type == "profileId")?.Value;

                    if (!string.IsNullOrEmpty(userIdClaim))
                    {
                        claimsIdentity?.AddClaim(new Claim(ClaimTypes.NameIdentifier, userIdClaim, profileIdClaim)); // Map userId to NameIdentifier
                    }
                    var userRolesClaim = context.Principal.Claims.FirstOrDefault(x => x.Type == "userRole")?.Value;
                    if (!string.IsNullOrEmpty(userRolesClaim))
                    {
                        var userRoles = JsonConvert.DeserializeObject<List<string>>(userRolesClaim);
                        foreach (var role in userRoles)
                        {
                            claimsIdentity?.AddClaim(new Claim(ClaimTypes.Role, role));
                        }
                    }
                }
                catch
                { }

                return Task.CompletedTask;
            }
        };
        return config;
    }
    public static RouteGroupBuilder RequireAuthorizationWithRole(this RouteGroupBuilder builder)
    {
        builder.RequireAuthorization(x => x.RequireClaim(ClaimTypes.Role));
        return builder;
    }
}
