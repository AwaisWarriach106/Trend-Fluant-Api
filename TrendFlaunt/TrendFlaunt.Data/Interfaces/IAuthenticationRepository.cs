using TrendFlaunt.Data.Models.RequestModel;

namespace TrendFlaunt.Data.Interfaces;

public interface IAuthenticationRepository
{
    Task<Guid> RegisterUserProfile(RegisterUserRequest request, CancellationToken ct);
}
