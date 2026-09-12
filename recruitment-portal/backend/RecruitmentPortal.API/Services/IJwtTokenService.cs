using RecruitmentPortal.API.Models;

namespace RecruitmentPortal.API.Services;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}
