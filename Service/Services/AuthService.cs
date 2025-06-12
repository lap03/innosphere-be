using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using Repository.Helpers;
using Service.Interfaces;
using Service.Models.AuthModels;
using Service.Models.EmailModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;
using Repository.Interfaces;

namespace Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IInitService initService, IConfiguration configuration, IEmailService emailService, ILogger<AuthService> logger, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<JwtModel> LoginAsync(LoginModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, model.Password);

            if (!isCorrect)
                throw new UnauthorizedAccessException("Invalid email or password");

            if (!existingUser.EmailConfirmed)
                throw new UnauthorizedAccessException("Email is not confirmed. Please verify your email before logging in.");

            if (existingUser.IsDeleted == true)
                throw new UnauthorizedAccessException("You have been banned");

            var accessToken = await CreateAccessToken(existingUser);
            var refreshToken = await CreateRefreshToken(existingUser);

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

        public async Task<JwtModel> LoginWithGoogleAsync(string idToken, string type, string? fullName, string? phoneNumber)
        {
            // Validate Google token
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["Authentication:Google:ClientId"] }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            if (payload == null)
                return null;

            // Map type to role
            string role = type?.Trim().ToLower() switch
            {
                "worker" => "Worker",
                "employer" => "Employer",
                _ => throw new ArgumentException("Invalid user type")
            };

            // Find or create user
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new User
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    FullName = fullName,
                    EmailConfirmed = true,
                    PhoneNumber = phoneNumber
                };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                    return null;
            }
            else if (!string.IsNullOrEmpty(phoneNumber) && user.PhoneNumber != phoneNumber)
            {
                user.PhoneNumber = phoneNumber;
                await _userManager.UpdateAsync(user);
            }

            // Gán role nếu chưa có
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(role, StringComparer.OrdinalIgnoreCase))
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            // Tạo JWT và refresh token
            var accessToken = await CreateAccessToken(user);
            var refreshToken = await CreateRefreshToken(user);

            return new JwtModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserID = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                AvatarUrl = user.AvatarUrl
            };
        }

        public async Task<IdentityResult> RegisterWorkerAsync(CancellationToken cancellationToken, User user)
        {
            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            if (!result.Succeeded)
                throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            await AssignRoleAsync(user, RolesHelper.Worker);

            await GenerateAndSendOtpAsync(user.Email, cancellationToken);

            return result;
        }

        public async Task<IdentityResult> RegisterEmployerAsync(CancellationToken cancellationToken, User user)
        {
            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            if (!result.Succeeded)
                throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            await AssignRoleAsync(user, RolesHelper.Employer);

            await GenerateAndSendOtpAsync(user.Email, cancellationToken);

            return result;
        }

        public async Task LogoutAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            // Xóa refresh token khỏi user
            await _userManager.RemoveAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken");
        }

        public async Task<bool> SendForgotPasswordOtpAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.EmailConfirmed)
                return false;

            var otp = new Random().Next(100000, 999999).ToString();
            var expiredAt = DateTime.UtcNow.AddMinutes(5);

            var emailOtpRepo = _unitOfWork.GetRepository<EmailOtp>();
            var oldOtps = (await emailOtpRepo.GetAllAsync(x => x.Email == email && !x.IsUsed && x.Purpose == "ForgotPassword")).ToList();
            if (oldOtps.Any())
                await emailOtpRepo.HardDeleteRange(oldOtps);

            var newOtp = new EmailOtp
            {
                Email = email,
                Otp = otp,
                ExpiredAt = expiredAt,
                IsUsed = false,
                Purpose = "ForgotPassword"
            };
            await emailOtpRepo.AddAsync(newOtp);
            await _unitOfWork.SaveChangesAsync();

            var subject = "Forgot Password OTP";
            var body = $"Your OTP code for password reset is: <b>{otp}</b>. This code is valid for 5 minutes.";
            await _emailService.SendMailAsync(cancellationToken, new EmailModel
            {
                To = email,
                Subject = subject,
                Body = body
            });

            return true;
        }

        public async Task<bool> ResendEmailOtpAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.EmailConfirmed)
                return false;

            await GenerateAndSendOtpAsync(email, cancellationToken);
            return true;
        }

        public async Task<bool> VerifyEmailOtpAsync(string email, string otp)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            if (user.EmailConfirmed)
                return true;

            var emailOtpRepo = _unitOfWork.GetRepository<EmailOtp>();
            var otpRecords = await emailOtpRepo.GetAllAsync(x =>
                x.Email == email && x.Otp == otp && !x.IsUsed && x.ExpiredAt > DateTime.UtcNow);
            var otpRecord = otpRecords.FirstOrDefault();

            if (otpRecord == null)
                throw new UnauthorizedAccessException("Invalid or expired OTP");

            otpRecord.IsUsed = true;
            await emailOtpRepo.Update(otpRecord);
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> VerifyForgotPasswordOtpAndResetAsync(string email, string otp, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.EmailConfirmed)
                return false;

            var emailOtpRepo = _unitOfWork.GetRepository<EmailOtp>();
            var otpRecords = await emailOtpRepo.GetAllAsync(x =>
                x.Email == email && x.Otp == otp && !x.IsUsed && x.ExpiredAt > DateTime.UtcNow && x.Purpose == "ForgotPassword");
            var otpRecord = otpRecords.FirstOrDefault();

            if (otpRecord == null)
                return false;

            otpRecord.IsUsed = true;
            await emailOtpRepo.Update(otpRecord);

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            await _unitOfWork.SaveChangesAsync();

            return result.Succeeded;
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

        //create refresh token for user
        public async Task<string> CreateRefreshToken(User user)
        {
            var code = Guid.NewGuid().ToString();
            var expiredDate = DateTime.UtcNow.AddHours(int.Parse(_configuration["JWT:RefreshTokenExpiredByHours"]));
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, code), // Unique ID của token
                new Claim(JwtRegisteredClaimNames.Exp, ((DateTimeOffset)expiredDate).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), // Thời gian hết hạn
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

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

            await _userManager.SetAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken", code);

            return token;
        }

        //validate refresh token and return new access token and refresh token
        public async Task<JwtModel> ValidateRefreshToken(RefreshJwtModel model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]);

            ClaimsPrincipal claimsPrincipal;
            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(model.RefreshToken, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);
            }
            catch
            {
                return null; // Token không hợp lệ hoặc đã hết hạn
            }

            var identity = claimsPrincipal.Identity as ClaimsIdentity;
            var email = identity?.FindFirst(JwtRegisteredClaimNames.Email)?.Value
                     ?? identity?.FindFirst(ClaimTypes.Email)?.Value;
            var jti = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(jti))
                return null;

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;

            var storedJti = await _userManager.GetAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken");
            if (string.IsNullOrEmpty(storedJti) || storedJti != jti)
                return null;

            var newAccessToken = await CreateAccessToken(user);
            var newRefreshToken = await CreateRefreshToken(user);

            return new JwtModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Email = user.Email,
                FullName = user.FullName,
                UserID = user.Id,
                AvatarUrl = user.AvatarUrl
            };
        }

        private async Task AssignRoleAsync(User user, string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            await _userManager.AddToRoleAsync(user, roleName);
        }

        private async Task<string> GenerateAndSendOtpAsync(string email, CancellationToken cancellationToken)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            var expiredAt = DateTime.UtcNow.AddMinutes(5);

            // Xoá các OTP cũ chưa dùng
            var emailOtpRepo = _unitOfWork.GetRepository<EmailOtp>();
            var oldOtps = (await emailOtpRepo.GetAllAsync(x => x.Email == email && !x.IsUsed)).ToList();
            if (oldOtps.Any())
            {
                await emailOtpRepo.HardDeleteRange(oldOtps);
            }

            // Lưu OTP mới
            var newOtp = new EmailOtp
            {
                Email = email,
                Otp = otp,
                ExpiredAt = expiredAt,
                IsUsed = false
            };
            await emailOtpRepo.AddAsync(newOtp);
            await _unitOfWork.SaveChangesAsync();

            // Gửi email
            var subject = "Your Email Verification OTP";
            var body = $"Your OTP code is: <b>{otp}</b>. This code is valid for 5 minutes.";
            await _emailService.SendMailAsync(cancellationToken, new EmailModel
            {
                To = email,
                Subject = subject,
                Body = body
            });

            return otp;
        }
    }
}
