using RecruitmentPortal.API.Models.DTOs;

namespace RecruitmentPortal.API.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}

/// <summary>Thrown for expected authentication failures (mapped to 400/401 by the controller).</summary>
public sealed class AuthException : Exception
{
    public AuthException(string message) : base(message) { }
}
