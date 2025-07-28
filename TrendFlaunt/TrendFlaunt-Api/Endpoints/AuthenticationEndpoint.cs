using Carter;
using TrendFlaunt_Api.Actions;
using TrendFlaunt_Api.Common.Routes;

namespace TrendFlaunt_Api.Endpoints;

public class AuthenticationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var groupAuthentication = app.MapGroup(AuthenticationRoutes.MapGroupAuthentication);

        groupAuthentication.MapPost(AuthenticationRoutes.Login, AuthenticationAction.Login).AllowAnonymous();
    }
}
