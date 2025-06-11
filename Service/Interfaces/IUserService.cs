using Service.Models.UserModels;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task<bool> BanAsync(string id);
        Task<IEnumerable<UserWithRoleModel>> GetAllAsync();
        Task<UserModel?> GetByIdAsync(string id);
        Task<bool> UnbanAsync(string id);
        Task<bool> UpdateAsync(UpdateUserModel model);
    }
}