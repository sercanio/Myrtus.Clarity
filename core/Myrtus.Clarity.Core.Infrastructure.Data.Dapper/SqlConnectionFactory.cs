using System.Data;
using Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;
using Npgsql;

namespace Myrtus.Clarity.Core.Infrastructure.Data.Dapper;

public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        return connection;
    }
}
