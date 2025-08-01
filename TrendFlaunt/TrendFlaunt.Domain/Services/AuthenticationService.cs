using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using TrendFlaunt.Data.Interfaces;
using TrendFlaunt.Data.Models.RequestModel;
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
    private readonly IAuthenticationRepository _authenticationRepository;
    public AuthenticationService(IConfiguration configuration, UserManager<IdentityUser> userManager, ITokenFactory tokenFactory, IAuthenticationRepository authenticationRepository)
    {
        _userManager = userManager;
        _tokenFactory = tokenFactory;
        _configuration = configuration;
        _authenticationRepository = authenticationRepository;
    }
    public async Task<ServiceResponse<Guid>> RegisterUser (RegisterUserRequest request, CancellationToken ct)
    {
        try
        {
            var user = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return ServiceResponse<Guid>.FailureResponse("Failed to register user.", ErrorCode.Error);
            }
            var assignRole = await _userManager.AddToRoleAsync(user, request.UserRole);

            request.UserId = user.Id;
            var userProfileId = await _authenticationRepository.RegisterUserProfile(request, ct);
            if (userProfileId == Guid.Empty)
            {
                return ServiceResponse<Guid>.FailureResponse("Failed to register user.", ErrorCode.Error);
            }
            return ServiceResponse<Guid>.SuccessResponse(userProfileId);
        }
        catch (Exception ex)
        {
            return ServiceResponse<Guid>.FailureResponse("Failed to register user.", ErrorCode.Error);
        }
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
    public async Task<ServiceResponse<UserSession>> LoginWithGoogle(GoogleLoginRequest request)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["GoogleAuth:ClientId"] }
            };
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);
            var email = payload.Email;
            var fullName = payload.Name;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new IdentityUser
                {
                    Email = email,
                    UserName = email,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return ServiceResponse<UserSession>.FailureResponse("Failed to register user with Google.", ErrorCode.Error);
                }
                await _authenticationRepository.RegisterUserProfile(new RegisterUserRequest
                {
                    UserId = user.Id,
                    FullName = fullName
                }, CancellationToken.None);
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || !roles.Any())
            {
                await _userManager.AddToRoleAsync(user, "User");
                roles = new List<string> { "User" };
            }

            var role = string.Join(", ", roles);
            var token = _tokenFactory.Generate(user.Id, user.Email, role);

            return ServiceResponse<UserSession>.SuccessResponse(token);
        }
        catch (Exception ex)
        {
            return ServiceResponse<UserSession>.FailureResponse("Google authentication failed.", ErrorCode.Error);
        }
    }
}
