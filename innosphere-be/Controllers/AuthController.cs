using innosphere_be.Models.Requests.AuthRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;
using Service.Models.AuthModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace innosphere_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authService.LoginAsync(model);
            return Ok(result);
        }

        [HttpPost("login-google")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginGoogle([FromBody] GoogleLoginRequest model)
        {
            var result = await _authService.LoginWithGoogleAsync(model.IdToken, model.Type, model.PhoneNumber);
            if (result == null)
                return Unauthorized(new { message = "Google login failed" });

            return Ok(result);
        }

        [HttpPost("register-worker")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterWorker(CancellationToken cancellationToken, RegisterRequest model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                AvatarUrl = model.AvatarUrl,
                Address = model.Address,
                PasswordHash = model.Password
            };
            
            await _authService.RegisterWorkerAsync(cancellationToken, user);

            return Ok(new
            {
                message = "Register successful. Please check your email for the OTP to verify your account."
            });
        }

        [HttpPost("register-employer")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterEmployer(CancellationToken cancellationToken, RegisterRequest model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                AvatarUrl = model.AvatarUrl,
                Address = model.Address,
                PasswordHash = model.Password
            };
            
            await _authService.RegisterEmployerAsync(cancellationToken, user);
            
            return Ok(new
            {
                message = "Register successful. Please check your email for the OTP to verify your account."
            });
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshJwtModel model)
        {
            var result = await _authService.ValidateRefreshToken(model);
            if (result == null)
                return Unauthorized(new { message = "Invalid or expired refresh token" });

            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value
                     ?? User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { message = "Invalid token: email not found" });

            await _authService.LogoutAsync(email);
            return Ok(new { message = "Logout successful" });
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] string email, CancellationToken cancellationToken)
        {
            var result = await _authService.SendForgotPasswordOtpAsync(email, cancellationToken);
            if (!result)
                return BadRequest(new { message = "Email not found or not confirmed." });

            return Ok(new { message = "OTP has been sent to your email. Please check your inbox." });
        }


        [HttpPost("resend-email-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendEmailOtp([FromBody] VerifyEmailOtpRequest model, CancellationToken cancellationToken)
        {
            var result = await _authService.ResendEmailOtpAsync(model.Email, cancellationToken);
            if (!result)
                return BadRequest(new { message = "Cannot resend OTP. Email may not exist or is already confirmed." });

            return Ok(new { message = "OTP has been resent. Please check your email." });
        }

        [HttpPost("verify-email-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmailOtp([FromBody] VerifyEmailOtpRequest model)
        {
            var result = await _authService.VerifyEmailOtpAsync(model.Email, model.Otp);
            if (!result)
                return BadRequest(new { message = "Invalid or expired OTP" });

            return Ok(new { message = "Email verified successfully" });
        }

        [HttpPost("reset-password-by-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordByOtp([FromBody] ResetPasswordByOtpRequest model)
        {
            var result = await _authService.VerifyForgotPasswordOtpAndResetAsync(model.Email, model.Otp, model.NewPassword);
            if (!result)
                return BadRequest(new { message = "Invalid OTP or failed to reset password." });

            return Ok(new { message = "Password has been reset successfully." });
        }
    }
}
