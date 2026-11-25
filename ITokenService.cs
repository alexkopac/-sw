using Auth.Data;
using Auth.DTO;

namespace Auth.Services
{
    public interface ITokenService
    {
        Task<AccessTokenResult> CreateAccessTokenAsync(AppUser user);
        Task<RefreshTokenResult> CreateRefreshTokenAsync(AppUser user);
        Task<AuthResponse?> RefreshAsync(string refreshToken);
        Task<bool> RevokeAsync(string refreshToken);
    }
}
