using TrendFlaunt.Data.Models.Dto;
using TrendFlaunt.Data.Models.RequestModel;

namespace TrendFlaunt.Data.Interfaces;

public interface IAuthenticationRepository
{
    Task<Guid> RegisterUserProfile(RegisterUserRequest request, CancellationToken ct);
    Task<List<UserProfileDto>> GetProfileByEmail(string email, CancellationToken ct);
}
