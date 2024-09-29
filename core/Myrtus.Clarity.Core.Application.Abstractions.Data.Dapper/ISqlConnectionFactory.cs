using System.Data;

namespace Myrtus.Clarity.Core.Application.Abstractions.Data.Dapper;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
