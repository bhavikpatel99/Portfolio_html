using System.ComponentModel.DataAnnotations;

namespace RecruitmentPortal.API.Models.DTOs;

/// <summary>Payload used to create or update a candidate profile/application.</summary>
public class CandidateRequest
{
    [Required, StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Phone, StringLength(30)]
    public string? Phone { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [StringLength(20)]
    public string? Gender { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? State { get; set; }

    [StringLength(100)]
    public string? Country { get; set; }

    [StringLength(20)]
    public string? PostalCode { get; set; }

    [StringLength(150)]
    public string? PositionApplied { get; set; }

    [Range(0, 60)]
    public decimal? TotalExperience { get; set; }

    [StringLength(150)]
    public string? CurrentCompany { get; set; }

    [Range(0, 100000000)]
    public decimal? CurrentCtc { get; set; }

    [Range(0, 100000000)]
    public decimal? ExpectedCtc { get; set; }

    [Range(0, 365)]
    public int? NoticePeriodDays { get; set; }

    [StringLength(150)]
    public string? HighestQualification { get; set; }

    [StringLength(1000)]
    public string? Skills { get; set; }

    [Url, StringLength(300)]
    public string? LinkedInUrl { get; set; }

    [Url, StringLength(300)]
    public string? ResumeUrl { get; set; }

    public string? CoverLetter { get; set; }
}
