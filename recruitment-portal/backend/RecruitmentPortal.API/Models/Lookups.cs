namespace RecruitmentPortal.API.Models;

/// <summary>Maps to dbo.Countries.</summary>
public class Country
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

/// <summary>Maps to dbo.States.</summary>
public class StateItem
{
    public int Id { get; set; }
    public int CountryId { get; set; }
    public string Name { get; set; } = string.Empty;
}
