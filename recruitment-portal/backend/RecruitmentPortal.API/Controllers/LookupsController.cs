using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecruitmentPortal.API.Models;
using RecruitmentPortal.API.Repositories;

namespace RecruitmentPortal.API.Controllers;

/// <summary>
/// Serves reference data that drives the form dropdowns
/// (countries, cascading states, and generic lookup categories).
/// These are non-sensitive so anonymous access is allowed.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class LookupsController : ControllerBase
{
    // Categories the client is allowed to query from dbo.LookupValues.
    private static readonly HashSet<string> AllowedCategories = new(StringComparer.OrdinalIgnoreCase)
    {
        "Gender", "MaritalStatus", "EmploymentType", "Source"
    };

    private readonly ILookupRepository _repository;

    public LookupsController(ILookupRepository repository)
    {
        _repository = repository;
    }

    /// <summary>All countries.</summary>
    [HttpGet("countries")]
    public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
    {
        return Ok(await _repository.GetCountriesAsync());
    }

    /// <summary>States/provinces for a given country.</summary>
    [HttpGet("countries/{countryId:int}/states")]
    public async Task<ActionResult<IEnumerable<StateItem>>> GetStates(int countryId)
    {
        return Ok(await _repository.GetStatesAsync(countryId));
    }

    /// <summary>Values for a generic lookup category (Gender, MaritalStatus, EmploymentType, Source).</summary>
    [HttpGet("categories/{category}")]
    public async Task<ActionResult<IEnumerable<string>>> GetCategory(string category)
    {
        if (!AllowedCategories.Contains(category))
            return NotFound(new { message = $"Unknown lookup category '{category}'." });

        return Ok(await _repository.GetLookupValuesAsync(category));
    }
}
