using Microsoft.AspNetCore.Identity;
using Repository.Entities;
using Service.Models.AuthModels;

namespace Service.Interfaces
{
    public interface IAuthService
    {
        Task<string> CreateAccessToken(User user);
        Task<string> CreateRefreshToken(User user);
        Task<JwtModel> LoginAsync(LoginModel model);
        Task LogoutAsync(string email);
        Task<IdentityResult> RegisterEmployerAsync(User user, int type);
        Task<IdentityResult> RegisterWorkerAsync(User user, int type);
        Task<JwtModel> ValidateRefreshToken(RefreshJwtModel model);
    }
}