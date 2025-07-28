using TrendFlaunt.Domain.ResponseModel;

namespace TrendFlaunt.Domain.Authentication;

public interface ITokenFactory
{
    UserSession Generate(string userId, string email, string userRoles);
    string CreateHash(string inputString);
    bool Compare(string input, string hash);
}
