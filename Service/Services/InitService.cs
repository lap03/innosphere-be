using Microsoft.AspNetCore.Identity;
using Repository.Entities;
using Repository.Helpers;
using Service.Interfaces;

namespace Service.Services
{
    public class InitService : IInitService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public InitService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Seed 3 user mặc định
        public async Task<IdentityResult> SeedUsersAsync()
        {
            // Kiểm tra nếu đã có bất kỳ user seed nào thì báo lỗi
            var adminExists = await _userManager.FindByEmailAsync("admin@example.com") != null;
            var employerExists = await _userManager.FindByEmailAsync("employer@example.com") != null;
            var workerExists = await _userManager.FindByEmailAsync("worker@example.com") != null;

            if (adminExists || employerExists || workerExists)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Seed users already exist. Seeding can only be performed once." });
            }

            // Nếu chưa có thì seed
            await CreateUserWithRoleAsync("admin@example.com", "1", RolesHelper.Admin);
            await CreateUserWithRoleAsync("employer@example.com", "1", RolesHelper.Employer);
            await CreateUserWithRoleAsync("worker@example.com", "1", RolesHelper.Worker);

            return IdentityResult.Success;
        }

        // Tạo user mới và gán role (nếu đã tồn tại thì bỏ qua)
        public async Task<IdentityResult> CreateUserWithRoleAsync(string email, string password, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
                return IdentityResult.Failed(new IdentityError { Description = "User already exists." });

            user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return result;

            if (await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            return IdentityResult.Success;
        }

        // Gán lại role cho user (mỗi user chỉ có 1 role)
        public async Task<IdentityResult> AssignRoleAsync(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            if (!await _roleManager.RoleExistsAsync(roleName))
                return IdentityResult.Failed(new IdentityError { Description = "Role not found." });

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            return await _userManager.AddToRoleAsync(user, roleName);
        }
    }
}
