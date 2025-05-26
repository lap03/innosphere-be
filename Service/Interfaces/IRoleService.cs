namespace Service.Interfaces
{
    public interface IRoleService
    {
        Task<bool> AddRoleAsync(string roleName);
        Task<bool> DeleteRoleIfUnusedAsync(string roleName);
        Task<List<string>> GetAllRolesAsync();
    }
}