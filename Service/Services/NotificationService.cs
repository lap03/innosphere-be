using AutoMapper;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using Service.Models.NotificationModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Lấy tất cả thông báo đang hoạt động (không bị xóa) – dành cho ADMIN.
        /// </summary>
        public async Task<List<NotificationModel>> GetAllActiveNotificationsForAdminAsync()
        {
            var repo = _unitOfWork.GetRepository<Notification>();
            var list = await repo.GetAllAsync(n => !n.IsDeleted);
            return _mapper.Map<List<NotificationModel>>(list);
        }

        /// <summary>
        /// Lấy tất cả thông báo của người dùng hiện tại (đã đăng nhập).
        /// </summary>
        public async Task<List<NotificationModel>> GetNotificationsByUserIdAsync(string userId)
        {
            var repo = _unitOfWork.GetRepository<Notification>();
            var list = await repo.GetAllAsync(n => !n.IsDeleted && n.UserId == userId);
            return _mapper.Map<List<NotificationModel>>(list);
        }

        /// <summary>
        /// Lấy chi tiết thông báo theo ID.
        /// </summary>
        public async Task<NotificationModel> GetNotificationByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Notification>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Notification not found.");
            return _mapper.Map<NotificationModel>(entity);
        }

        /// <summary>
        /// Tạo mới một thông báo cho người dùng hiện tại.
        /// </summary>
        public async Task<NotificationModel> CreateNotificationAsync(CreateNotificationModel dto, string userId)
        {
            var repo = _unitOfWork.GetRepository<Notification>();
            var entity = _mapper.Map<Notification>(dto);
            entity.UserId = userId;
            await repo.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<NotificationModel>(entity);
        }

        /// <summary>
        /// Cập nhật nội dung thông báo.
        /// </summary>
        public async Task<NotificationModel> UpdateNotificationAsync(int id, UpdateNotificationModel dto)
        {
            var repo = _unitOfWork.GetRepository<Notification>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Notification not found.");

            _mapper.Map(dto, entity);
            await repo.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<NotificationModel>(entity);
        }

        /// <summary>
        /// Xóa mềm thông báo (ẩn khỏi danh sách).
        /// </summary>
        public async Task<bool> SoftDeleteNotificationAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Notification>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Notification not found.");

            await repo.SoftDelete(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Khôi phục lại thông báo đã bị xóa mềm.
        /// </summary>
        public async Task<bool> RestoreNotificationAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Notification>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Notification not found.");

            entity.IsDeleted = false;
            await repo.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Xóa cứng thông báo khỏi database.
        /// </summary>
        public async Task<bool> HardDeleteNotificationAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Notification>();
            var entity = await repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Notification not found.");

            await repo.HardDelete(n => n.Id == id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
