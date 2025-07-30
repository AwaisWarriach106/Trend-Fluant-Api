using Dapper;
using System.Data;
using TrendFlaunt.Data.Interfaces;
using TrendFlaunt.Data.Models.RequestModel;
using TrendFlaunt.Data.Queries;

namespace TrendFlaunt.Data.Repository;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly IDbClient _dbClient;
    public AuthenticationRepository(IDbClient dbClient)
    {
        _dbClient = dbClient;
    }
    public async Task<Guid> RegisterUserProfile(RegisterUserRequest request, CancellationToken ct)
    {
        var command = new CommandDefinition(
            AuthenticationQueries.ManualUserRegistration,
            new
            {
                UserId = request.UserId,
                FullName = request.FullName,
                UserType = request.UserType.ToString(),
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
            },
            commandType: CommandType.Text,
            cancellationToken: ct
        );
        var result = await _dbClient.QueryAsync<Guid>(command);
        return result.FirstOrDefault();

    }
}
