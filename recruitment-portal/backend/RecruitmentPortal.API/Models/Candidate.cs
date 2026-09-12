namespace RecruitmentPortal.API.Models;

/// <summary>Maps to dbo.Candidates.</summary>
public class Candidate
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? PositionApplied { get; set; }
    public decimal? TotalExperience { get; set; }
    public string? CurrentCompany { get; set; }
    public decimal? CurrentCtc { get; set; }
    public decimal? ExpectedCtc { get; set; }
    public int? NoticePeriodDays { get; set; }
    public string? HighestQualification { get; set; }
    public string? Skills { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? ResumeUrl { get; set; }
    public string? CoverLetter { get; set; }
    public string Status { get; set; } = "Applied";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
