using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using Repository.Helpers;
using Service.Models.AuthModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<JwtModel> LoginAsync(LoginModel model)
        {

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser == null) throw new UnauthorizedAccessException("Invalid email or password");

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, model.Password);
            if (!isCorrect) throw new UnauthorizedAccessException("Invalid email or password");

            if (existingUser.IsDeleted == true) throw new UnauthorizedAccessException("You have been banned");

            var accessToken = await CreateAccessToken(existingUser);
            //var refreshToken = await CreateRefreshToken(existingUser);
            var refreshToken = string.Empty;

            return new JwtModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserID = existingUser.Id,
                Email = existingUser.Email,
                FullName = existingUser.FullName,
                AvatarUrl = existingUser.AvatarUrl
            };
        }

        public async Task<IdentityResult> RegisterAsync(User user, int type)
        {

            var result = await _userManager.CreateAsync(user, user.PasswordHash);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            //await AssignRoleAsync(user, RolesHelper.Worker);

            return result;
        }


        //create access token for user
        public async Task<string> CreateAccessToken(User user)
        {
            DateTime expiredDate = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JWT:AccessTokenExpiredByMinutes"]));
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique ID của token
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), // Thời gian phát hành
                new Claim(JwtRegisteredClaimNames.Exp, ((DateTimeOffset)expiredDate).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), // Thời gian hết hạn
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenInfo = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: expiredDate,
                    signingCredentials: credential
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenInfo);

            return token;
        }
    }
}
