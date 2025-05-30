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
        Task<JwtModel> LoginWithGoogleAsync(string idToken, string type, string? phoneNumber);
        Task LogoutAsync(string email);
        Task<IdentityResult> RegisterEmployerAsync(CancellationToken cancellationToken, User user);
        Task<IdentityResult> RegisterWorkerAsync(CancellationToken cancellationToken, User user);
        Task<bool> ResendEmailOtpAsync(string email, CancellationToken cancellationToken);
        Task<bool> SendForgotPasswordOtpAsync(string email, CancellationToken cancellationToken);
        Task<JwtModel> ValidateRefreshToken(RefreshJwtModel model);
        Task<bool> VerifyEmailOtpAsync(string email, string otp);
        Task<bool> VerifyForgotPasswordOtpAndResetAsync(string email, string otp, string newPassword);
    }
}