using Dapper;
using RecruitmentPortal.API.Data;
using RecruitmentPortal.API.Models;

namespace RecruitmentPortal.API.Repositories;

/// <summary>Dapper-based data access for the lookup tables that drive the form dropdowns.</summary>
public sealed class LookupRepository : ILookupRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public LookupRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Country>> GetCountriesAsync()
    {
        const string sql = "SELECT Id, Code, Name FROM dbo.Countries ORDER BY Name;";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Country>(sql);
    }

    public async Task<IEnumerable<StateItem>> GetStatesAsync(int countryId)
    {
        const string sql = @"SELECT Id, CountryId, Name
                             FROM dbo.States
                             WHERE CountryId = @CountryId
                             ORDER BY Name;";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<StateItem>(sql, new { CountryId = countryId });
    }

    public async Task<IEnumerable<string>> GetLookupValuesAsync(string category)
    {
        const string sql = @"SELECT Value
                             FROM dbo.LookupValues
                             WHERE Category = @Category
                             ORDER BY SortOrder, Value;";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<string>(sql, new { Category = category });
    }
}
