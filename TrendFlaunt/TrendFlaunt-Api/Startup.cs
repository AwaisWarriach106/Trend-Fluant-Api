using Carter;
using Microsoft.AspNetCore.Identity;
using TrendFlaunt_Api.StartupExtensions;

namespace TrendFlaunt_Api;

public class Startup
{
    public static void Configuration(IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddControllers().AddApplicationPart(typeof(Startup).Assembly);
        services.AddSwaggerGen();
        services.AddCarter();
        services.AddScoped<IPasswordHasher<IdentityUser>, PasswordHasher<IdentityUser>>();
        services.AddHttpContextAccessor();
        services.AddTrendFlauntConfiguration(configuration);
        services.AddCors();
        services.AddProblemDetails();
    }
    public static void Configure(WebApplication application)
    {
        application.UseSwagger();
        application.UseSwaggerUI(x =>
        {
            x.DisplayRequestDuration();
        });
        application.UseStaticFiles();
        application.UseAuthorization();
        application.UseHttpsRedirection();
        application.MapCarter();
        application.UseCors(builder => builder
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowAnyMethod());
    }
}
