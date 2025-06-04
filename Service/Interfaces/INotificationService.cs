using Service.Models.NotificationModels;

namespace Service.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationModel> CreateAsync(CreateNotificationModel dto);
        Task<bool> DeleteAsync(int id);
        Task<List<NotificationModel>> GetAllActiveAsync();
        Task<List<NotificationModel>> GetAllAsync();
        Task<NotificationModel> GetByIdAsync(int id);
        Task<bool> HardDeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
        Task<NotificationModel> UpdateAsync(int id, UpdateNotificationModel dto);
    }
}