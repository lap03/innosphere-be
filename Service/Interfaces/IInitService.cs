using Microsoft.AspNetCore.Identity;

namespace Service.Interfaces
{
    public interface IInitService
    {
        Task<IdentityResult> AssignRoleAsync(string email, string roleName);
        Task<IdentityResult> CreateUserWithRoleAsync(string email, string password, string role);
        Task<IdentityResult> SeedUsersAsync();
    }
}