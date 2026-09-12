using RecruitmentPortal.API.Models;

namespace RecruitmentPortal.API.Repositories;

public interface ICandidateRepository
{
    Task<IEnumerable<Candidate>> GetAllAsync();
    Task<Candidate?> GetByIdAsync(int id);
    Task<int> CreateAsync(Candidate candidate);
    Task<bool> UpdateAsync(Candidate candidate);
    Task<bool> DeleteAsync(int id);
}
