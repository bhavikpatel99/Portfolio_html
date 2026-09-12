using System.Data;

namespace RecruitmentPortal.API.Data;

/// <summary>
/// Creates open ADO.NET connections to SQL Server for Dapper to use.
/// </summary>
public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
