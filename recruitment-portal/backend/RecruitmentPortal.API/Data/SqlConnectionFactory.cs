using System.Data;
using Microsoft.Data.SqlClient;

namespace RecruitmentPortal.API.Data;

/// <summary>
/// SQL Server (mssqlclient) implementation of <see cref="IDbConnectionFactory"/>.
/// Uses the "DefaultConnection" connection string from configuration.
/// </summary>
public sealed class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
    }

    public IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
