using Dapper;
using System.Data;

namespace TrendFlaunt.Data.Interfaces;

public interface IDbClient
{
    Task<IEnumerable<T>> QueryAsync<T>(CommandDefinition commandDefinition);
    Task<int> ExecuteAsync(CommandDefinition commandDefinition);
    Task<T?> ExecuteScalarAsync<T>(CommandDefinition commandDefinition);
    Task<int> ExecuteAsync(string sql, object? param = null, CommandType? commandType = null);
    IDbConnection OpenConnection();
}
