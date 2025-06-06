using Service.Models.SubscriptionModels;

namespace Service.Interfaces
{
    public interface ISubscriptionService
    {
        Task<bool> CanPostJobAsync(int subscriptionId);
        Task<SubscriptionModel> CreateAsync(CreateSubscriptionModel dto);
        Task<bool> DeleteAsync(int id);
        Task<List<SubscriptionModel>> GetAllAsync();
        Task<SubscriptionModel> GetByIdAsync(int id);
        Task<bool> HardDeleteAsync(int id);
        Task<SubscriptionModel> PurchaseSubscriptionAsync(CreateSubscriptionModel dto, bool forceReplace = false);
        Task<bool> RestoreAsync(int id);
        Task<SubscriptionModel> UpdateAsync(int id, UpdateSubscriptionModel dto);
    }
}