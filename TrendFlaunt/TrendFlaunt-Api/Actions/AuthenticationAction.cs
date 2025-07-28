using Microsoft.AspNetCore.Mvc;
using TrendFlaunt.Domain.Interfaces;
using TrendFlaunt.Domain.ResponseModel;
using TrendFlaunt_Api.StartupExtensions;

namespace TrendFlaunt_Api.Actions;

public class AuthenticationAction
{
    public static async Task<IResult> Login(LoginUserModel loginUser,
   [FromServices] IAuthenticationService authenticationService)
    {
        var serviceResponse = await authenticationService.Login(loginUser);
        return serviceResponse.Success ? Results.Ok(serviceResponse) : serviceResponse.ToProblemDetails();
    }
}
