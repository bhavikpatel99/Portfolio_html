using Dapper;
using RecruitmentPortal.API.Data;
using RecruitmentPortal.API.Models;

namespace RecruitmentPortal.API.Repositories;

/// <summary>Dapper-based data access for dbo.Candidates.</summary>
public sealed class CandidateRepository : ICandidateRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    private const string SelectColumns = @"
        Id, UserId, FirstName, MiddleName, LastName, Email, Phone, AlternatePhone, DateOfBirth,
        Gender, MaritalStatus, Nationality, Address, City, State, Country, PostalCode,
        PositionApplied, EmploymentType, TotalExperience, CurrentCompany, CurrentCtc, ExpectedCtc,
        NoticePeriodDays, PreferredLocation, WillingToRelocate, AvailableFrom, HighestQualification,
        Skills, LinkedInUrl, PortfolioUrl, GitHubUrl, ResumeUrl, CoverLetter, ReferenceName,
        ReferenceContact, Source, Status, CreatedAt, UpdatedAt";

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
            (UserId, FirstName, MiddleName, LastName, Email, Phone, AlternatePhone, DateOfBirth,
             Gender, MaritalStatus, Nationality, Address, City, State, Country, PostalCode,
             PositionApplied, EmploymentType, TotalExperience, CurrentCompany, CurrentCtc, ExpectedCtc,
             NoticePeriodDays, PreferredLocation, WillingToRelocate, AvailableFrom, HighestQualification,
             Skills, LinkedInUrl, PortfolioUrl, GitHubUrl, ResumeUrl, CoverLetter, ReferenceName,
             ReferenceContact, Source, Status)
            VALUES
            (@UserId, @FirstName, @MiddleName, @LastName, @Email, @Phone, @AlternatePhone, @DateOfBirth,
             @Gender, @MaritalStatus, @Nationality, @Address, @City, @State, @Country, @PostalCode,
             @PositionApplied, @EmploymentType, @TotalExperience, @CurrentCompany, @CurrentCtc, @ExpectedCtc,
             @NoticePeriodDays, @PreferredLocation, @WillingToRelocate, @AvailableFrom, @HighestQualification,
             @Skills, @LinkedInUrl, @PortfolioUrl, @GitHubUrl, @ResumeUrl, @CoverLetter, @ReferenceName,
             @ReferenceContact, @Source, @Status);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, candidate);
    }

    public async Task<bool> UpdateAsync(Candidate candidate)
    {
        const string sql = @"
            UPDATE dbo.Candidates SET
                FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, Email = @Email,
                Phone = @Phone, AlternatePhone = @AlternatePhone, DateOfBirth = @DateOfBirth,
                Gender = @Gender, MaritalStatus = @MaritalStatus, Nationality = @Nationality,
                Address = @Address, City = @City, State = @State, Country = @Country, PostalCode = @PostalCode,
                PositionApplied = @PositionApplied, EmploymentType = @EmploymentType,
                TotalExperience = @TotalExperience, CurrentCompany = @CurrentCompany, CurrentCtc = @CurrentCtc,
                ExpectedCtc = @ExpectedCtc, NoticePeriodDays = @NoticePeriodDays,
                PreferredLocation = @PreferredLocation, WillingToRelocate = @WillingToRelocate,
                AvailableFrom = @AvailableFrom, HighestQualification = @HighestQualification, Skills = @Skills,
                LinkedInUrl = @LinkedInUrl, PortfolioUrl = @PortfolioUrl, GitHubUrl = @GitHubUrl,
                ResumeUrl = @ResumeUrl, CoverLetter = @CoverLetter, ReferenceName = @ReferenceName,
                ReferenceContact = @ReferenceContact, Source = @Source, Status = @Status,
                UpdatedAt = SYSUTCDATETIME()
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
