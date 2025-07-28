using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;
using TrendFlaunt.Data.Interfaces;
using TrendFlaunt.Data.Settings;

namespace TrendFlaunt.Data.Client;

public class DbClient : IDbClient
{
    private readonly PostgresConfiguration _config;

    public DbClient(IOptions<PostgresConfiguration> config)
    {
        _config = config.Value;
    }

    public IDbConnection OpenConnection()
    {
        var sqlConnection = new NpgsqlConnection(_config.ConnectionString);

        return sqlConnection;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(CommandDefinition commandDefinition)
    {
        using var connection = OpenConnection();

        return await connection.QueryAsync<T>(commandDefinition);
    }

    public async Task<T?> ExecuteScalarAsync<T>(CommandDefinition commandDefinition)
    {
        using var connection = OpenConnection();

        return await connection.ExecuteScalarAsync<T>(commandDefinition);
    }

    public async Task<int> ExecuteAsync(CommandDefinition commandDefinition)
    {
        using var connection = OpenConnection();

        return await connection.ExecuteAsync(commandDefinition);
    }

    public async Task<int> ExecuteAsync(string sql, object? param = null, CommandType? commandType = null)
    {
        using var connection = OpenConnection();

        return await connection.ExecuteAsync(sql, param, commandType: commandType);
    }
}
