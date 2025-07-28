using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TrendFlaunt.Domain.Authentication;
using TrendFlaunt.Domain.Common;
using TrendFlaunt.Domain.Interfaces;
using TrendFlaunt.Domain.ResponseModel;

namespace TrendFlaunt.Domain.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenFactory _tokenFactory;
    private readonly IConfiguration _configuration;
    public AuthenticationService(IConfiguration configuration, UserManager<IdentityUser> userManager, ITokenFactory tokenFactory)
    {
        _userManager = userManager;
        _tokenFactory = tokenFactory;
        _configuration = configuration;
    }
    public async Task<ServiceResponse<UserSession>> Login(LoginUserModel loginModel)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user == null)
                return ServiceResponse<UserSession>.FailureResponse("Invalid username or password.", ErrorCode.NotFound);

            if (await _userManager.IsLockedOutAsync(user))
                return ServiceResponse<UserSession>.FailureResponse("Account is locked. Please try again later.", ErrorCode.Locked);

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginModel.Password);
            if (!isPasswordValid)
            {
                var lockoutCount = await _userManager.AccessFailedAsync(user);
                if (await _userManager.IsLockedOutAsync(user))
                {
                    return ServiceResponse<UserSession>.FailureResponse("Account is locked due to multiple failed login attempts. Please try again later.", ErrorCode.Locked);
                }

                return ServiceResponse<UserSession>.FailureResponse("Invalid username or password.", ErrorCode.NotFound);
            }
            var resetLockoutCount = await _userManager.ResetAccessFailedCountAsync(user);
            var twoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
                var userRoles = await _userManager.GetRolesAsync(user);
               

            if (userRoles == null || !userRoles.Any())
                    return ServiceResponse<UserSession>.FailureResponse("User does not belong to any role.", ErrorCode.UnauthorizedAccess);
            var role = string.Join(", ", userRoles);
            var token = _tokenFactory.Generate(user.Id, user.Email ?? " ", role);
                return ServiceResponse<UserSession>.SuccessResponse(token);
        }
        catch (Exception ex)
        {
            return ServiceResponse<UserSession>.FailureResponse("Error occured while login!", ErrorCode.Error);
        }
    }
}
