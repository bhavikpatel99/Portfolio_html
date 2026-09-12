using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentPortal.API.Models;
using RecruitmentPortal.API.Models.DTOs;
using RecruitmentPortal.API.Repositories;

namespace RecruitmentPortal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // all candidate endpoints require a valid JWT
public class CandidatesController : ControllerBase
{
    private readonly ICandidateRepository _repository;

    public CandidatesController(ICandidateRepository repository)
    {
        _repository = repository;
    }

    /// <summary>List all candidate applications.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Candidate>>> GetAll()
    {
        var candidates = await _repository.GetAllAsync();
        return Ok(candidates);
    }

    /// <summary>Get a single candidate by id.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Candidate>> GetById(int id)
    {
        var candidate = await _repository.GetByIdAsync(id);
        return candidate is null ? NotFound() : Ok(candidate);
    }

    /// <summary>Submit a new candidate application form.</summary>
    [HttpPost]
    public async Task<ActionResult<Candidate>> Create([FromBody] CandidateRequest request)
    {
        var candidate = MapToEntity(request);
        candidate.UserId = GetCurrentUserId();
        candidate.Status = "Applied";

        candidate.Id = await _repository.CreateAsync(candidate);
        return CreatedAtAction(nameof(GetById), new { id = candidate.Id }, candidate);
    }

    /// <summary>Update an existing candidate application.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CandidateRequest request)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
            return NotFound();

        var candidate = MapToEntity(request);
        candidate.Id = id;
        candidate.UserId = existing.UserId;
        candidate.Status = existing.Status;

        var updated = await _repository.UpdateAsync(candidate);
        return updated ? NoContent() : NotFound();
    }

    /// <summary>Delete a candidate application.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }

    private int? GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out var id) ? id : null;
    }

    private static Candidate MapToEntity(CandidateRequest r) => new()
    {
        FirstName = r.FirstName,
        MiddleName = r.MiddleName,
        LastName = r.LastName,
        Email = r.Email,
        Phone = r.Phone,
        AlternatePhone = r.AlternatePhone,
        DateOfBirth = r.DateOfBirth,
        Gender = r.Gender,
        MaritalStatus = r.MaritalStatus,
        Nationality = r.Nationality,
        Address = r.Address,
        City = r.City,
        State = r.State,
        Country = r.Country,
        PostalCode = r.PostalCode,
        PositionApplied = r.PositionApplied,
        EmploymentType = r.EmploymentType,
        TotalExperience = r.TotalExperience,
        CurrentCompany = r.CurrentCompany,
        CurrentCtc = r.CurrentCtc,
        ExpectedCtc = r.ExpectedCtc,
        NoticePeriodDays = r.NoticePeriodDays,
        PreferredLocation = r.PreferredLocation,
        WillingToRelocate = r.WillingToRelocate,
        AvailableFrom = r.AvailableFrom,
        HighestQualification = r.HighestQualification,
        Skills = r.Skills,
        LinkedInUrl = r.LinkedInUrl,
        PortfolioUrl = r.PortfolioUrl,
        GitHubUrl = r.GitHubUrl,
        ResumeUrl = r.ResumeUrl,
        CoverLetter = r.CoverLetter,
        ReferenceName = r.ReferenceName,
        ReferenceContact = r.ReferenceContact,
        Source = r.Source
    };
}
