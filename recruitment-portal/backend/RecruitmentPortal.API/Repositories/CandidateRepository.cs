using Dapper;
using RecruitmentPortal.API.Data;
using RecruitmentPortal.API.Models;

namespace RecruitmentPortal.API.Repositories;

/// <summary>Dapper-based data access for dbo.Candidates.</summary>
public sealed class CandidateRepository : ICandidateRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    private const string SelectColumns = @"
        Id, UserId, FirstName, LastName, Email, Phone, DateOfBirth, Gender,
        Address, City, State, Country, PostalCode, PositionApplied, TotalExperience,
        CurrentCompany, CurrentCtc, ExpectedCtc, NoticePeriodDays, HighestQualification,
        Skills, LinkedInUrl, ResumeUrl, CoverLetter, Status, CreatedAt, UpdatedAt";

    public CandidateRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Candidate>> GetAllAsync()
    {
        var sql = $"SELECT {SelectColumns} FROM dbo.Candidates ORDER BY CreatedAt DESC;";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryAsync<Candidate>(sql);
    }

    public async Task<Candidate?> GetByIdAsync(int id)
    {
        var sql = $"SELECT {SelectColumns} FROM dbo.Candidates WHERE Id = @Id;";
        using var connection = _connectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Candidate>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Candidate candidate)
    {
        const string sql = @"
            INSERT INTO dbo.Candidates
            (UserId, FirstName, LastName, Email, Phone, DateOfBirth, Gender, Address, City, State,
             Country, PostalCode, PositionApplied, TotalExperience, CurrentCompany, CurrentCtc,
             ExpectedCtc, NoticePeriodDays, HighestQualification, Skills, LinkedInUrl, ResumeUrl,
             CoverLetter, Status)
            VALUES
            (@UserId, @FirstName, @LastName, @Email, @Phone, @DateOfBirth, @Gender, @Address, @City, @State,
             @Country, @PostalCode, @PositionApplied, @TotalExperience, @CurrentCompany, @CurrentCtc,
             @ExpectedCtc, @NoticePeriodDays, @HighestQualification, @Skills, @LinkedInUrl, @ResumeUrl,
             @CoverLetter, @Status);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, candidate);
    }

    public async Task<bool> UpdateAsync(Candidate candidate)
    {
        const string sql = @"
            UPDATE dbo.Candidates SET
                FirstName = @FirstName, LastName = @LastName, Email = @Email, Phone = @Phone,
                DateOfBirth = @DateOfBirth, Gender = @Gender, Address = @Address, City = @City,
                State = @State, Country = @Country, PostalCode = @PostalCode,
                PositionApplied = @PositionApplied, TotalExperience = @TotalExperience,
                CurrentCompany = @CurrentCompany, CurrentCtc = @CurrentCtc, ExpectedCtc = @ExpectedCtc,
                NoticePeriodDays = @NoticePeriodDays, HighestQualification = @HighestQualification,
                Skills = @Skills, LinkedInUrl = @LinkedInUrl, ResumeUrl = @ResumeUrl,
                CoverLetter = @CoverLetter, Status = @Status, UpdatedAt = SYSUTCDATETIME()
            WHERE Id = @Id;";

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, candidate);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM dbo.Candidates WHERE Id = @Id;";
        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
