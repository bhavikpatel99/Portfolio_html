using RecruitmentPortal.API.Models;

namespace RecruitmentPortal.API.Repositories;

public interface ILookupRepository
{
    Task<IEnumerable<Country>> GetCountriesAsync();
    Task<IEnumerable<StateItem>> GetStatesAsync(int countryId);
    Task<IEnumerable<string>> GetLookupValuesAsync(string category);
}
