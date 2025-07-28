using TrendFlaunt.Domain.Common;
using TrendFlaunt.Domain.ResponseModel;

namespace TrendFlaunt.Domain.Interfaces;

public interface IAuthenticationService
{
    Task<ServiceResponse<UserSession>> Login(LoginUserModel loginModel);
}
