using Service.Models.SubscriptionPackageModels;

namespace Service.Interfaces
{
    public interface ISubscriptionPackageService
    {
        Task<SubscriptionPackageModel> CreateAsync(CreateSubscriptionPackageModel dto);
        Task<bool> DeleteAsync(int id);
        Task<List<SubscriptionPackageModel>> GetAllActiveAsync();
        Task<List<SubscriptionPackageModel>> GetAllAsync();
        Task<SubscriptionPackageModel> GetByIdAsync(int id);
        Task<bool> HardDeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
        Task<SubscriptionPackageModel> UpdateAsync(int id, UpdateSubscriptionPackageModel dto);
    }
}