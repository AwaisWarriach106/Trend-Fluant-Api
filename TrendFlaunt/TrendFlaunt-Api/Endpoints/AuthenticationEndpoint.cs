using Carter;
using TrendFlaunt_Api.Actions;
using TrendFlaunt_Api.Common.Routes;
using TrendFlaunt_Api.StartupExtensions;

namespace TrendFlaunt_Api.Endpoints;

public class AuthenticationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var groupAuthentication = app.MapGroup(AuthenticationRoutes.MapGroupAuthentication).RequireAuthorizationWithRole();

        groupAuthentication.MapPost(AuthenticationRoutes.Login, AuthenticationAction.Login).AllowAnonymous();
        groupAuthentication.MapPost(AuthenticationRoutes.RegisterUser, AuthenticationAction.RegisterUser).AllowAnonymous();
        groupAuthentication.MapPost(AuthenticationRoutes.LoginWithGoogle, AuthenticationAction.LoginWithGoogle).AllowAnonymous();
    }
}
