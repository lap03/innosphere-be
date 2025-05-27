using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Repository.Entities;
using Service.Models.AuthModels;
using Service.Interfaces;
using innosphere_be.Models.Requests.AuthRequest;
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

        [HttpPost("register-worker")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterWorker([FromBody] RegisterRequest model)
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
            var result = await _authService.RegisterWorkerAsync(user, 0);
            return Ok(result);
        }

        [HttpPost("register-employer")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterEmployer([FromBody] RegisterRequest model)
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
            var result = await _authService.RegisterEmployerAsync(user, 0);
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
    }
}
