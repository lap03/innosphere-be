using Service.Models.NotificationModels;

namespace Service.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationModel> CreateNotificationAsync(CreateNotificationModel dto, string userId);
        Task<List<NotificationModel>> GetAllActiveNotificationsForAdminAsync();
        Task<NotificationModel> GetNotificationByIdAsync(int id);
        Task<List<NotificationModel>> GetNotificationsByUserIdAsync(string userId);
        Task<bool> HardDeleteNotificationAsync(int id);
        Task<bool> RestoreNotificationAsync(int id);
        Task<bool> SoftDeleteNotificationAsync(int id);
        Task<NotificationModel> UpdateNotificationAsync(int id, UpdateNotificationModel dto);
    }
}