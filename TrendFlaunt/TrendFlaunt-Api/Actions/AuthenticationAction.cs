using Microsoft.AspNetCore.Mvc;
using TrendFlaunt.Data.Models.RequestModel;
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
    public static async Task<IResult> RegisterUser(RegisterUserRequest request,
 [FromServices] IAuthenticationService authenticationService)
    {
        var serviceResponse = await authenticationService.RegisterUser(request);
        return serviceResponse.Success ? Results.Ok(serviceResponse) : serviceResponse.ToProblemDetails();
    }
    public static async Task<IResult> LoginWithGoogle(GoogleLoginRequest loginUser,
  [FromServices] IAuthenticationService authenticationService)
    {
        var serviceResponse = await authenticationService.LoginWithGoogle(loginUser);
        return serviceResponse.Success ? Results.Ok(serviceResponse) : serviceResponse.ToProblemDetails();
    }
}
