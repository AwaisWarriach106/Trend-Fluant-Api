using TrendFlaunt.Data.Models.RequestModel;
using TrendFlaunt.Domain.Common;
using TrendFlaunt.Domain.ResponseModel;

namespace TrendFlaunt.Domain.Interfaces;

public interface IAuthenticationService
{
    Task<ServiceResponse<UserSession>> Login(LoginUserModel loginModel);
    Task<ServiceResponse<Guid>> RegisterUser(RegisterUserRequest request, CancellationToken ct = default);
    Task<ServiceResponse<UserSession>> LoginWithGoogle(GoogleLoginRequest request);
}
