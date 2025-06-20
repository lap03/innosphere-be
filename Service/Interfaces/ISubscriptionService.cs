using Service.Models.SubscriptionModels;

namespace Service.Interfaces
{
    public interface ISubscriptionService
    {
        Task<bool> CancelSubscriptionAsync(int subscriptionId, int employerId);
        Task<bool> CanPostJobAsync(int employerId);
        Task<List<SubscriptionModel>> GetAllByEmployerAsync(int employerId);
        Task<SubscriptionModel> GetByIdAsync(int id);
        Task<bool> HardDeleteAsync(int subscriptionId, int employerId);
        Task<SubscriptionModel> PurchaseSubscriptionAsync(CreateSubscriptionModel dto);

        // Admin methods
        Task<List<SubscriptionModel>> GetAllForAdminAsync();
    }
}